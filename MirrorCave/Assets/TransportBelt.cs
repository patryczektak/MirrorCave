using System.Collections.Generic;
using UnityEngine;

public class TransportBelt : Interactable
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private Vector3 direction = Vector3.forward;
    [SerializeField] private List<Item> itemsOnBelt = new List<Item>();
    [SerializeField] private float maxItemDistance = 0.5f;

    private void Update()
    {
        // Move each item along the belt
        foreach (Item item in itemsOnBelt)
        {
            if (item != null && item.itemState == Item.State.Loose)
            {
                item.transform.position += direction * speed * Time.deltaTime;
            }
        }

        // Remove null items from the list
        itemsOnBelt.RemoveAll(item => item == null);
    }

    public override bool CanInteract(PlayerController player)
    {
        // If the player has an item, they can put it on the belt
        if (player.carriedItem != null)
        {
            interactActionName = "Put Item on Belt";
            return true;
        }

        // If the player doesn't have an item, they can take the closest one from the belt
        foreach (Item item in itemsOnBelt)
        {
            if (item != null && item.itemState == Item.State.Loose &&
                Vector3.Distance(player.transform.position, item.transform.position) < maxItemDistance)
            {
                interactActionName = "Take Item from Belt";
                return true;
            }
        }

        return false;
    }

    public override void Interact(PlayerController player)
    {
        // If the player has an item, put it on the belt
        if (player.carriedItem != null)
        {
            player.carriedItem.itemState = Item.State.Loose;
            itemsOnBelt.Add(player.carriedItem);
            player.carriedItem = null;
            return;
        }

        // If the player doesn't have an item, take the closest one from the belt
        Item closestItem = null;
        float closestDistance = maxItemDistance;
        foreach (Item item in itemsOnBelt)
        {
            float distance = Vector3.Distance(player.transform.position, item.transform.position);
            if (item != null && item.itemState == Item.State.Loose && distance < closestDistance)
            {
                closestItem = item;
                closestDistance = distance;
            }
        }

        if (closestItem != null)
        {
            closestItem.itemState = Item.State.Picked;
            player.carriedItem = closestItem;
            itemsOnBelt.Remove(closestItem);
        }
    }
}
