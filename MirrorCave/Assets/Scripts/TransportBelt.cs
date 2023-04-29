using System.Collections.Generic;
using UnityEngine;

public class TransportBelt : Interactable
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private Vector3 direction = Vector3.forward;
    [SerializeField] private float maxTravelDistance = 5f;
    [SerializeField] private float maxItemDistance = 0.5f;
    [SerializeField] private float hoverHeight = 1f;

    private Dictionary<Item, float> itemsOnBelt = new Dictionary<Item, float>();

    private void Update()
    {
        List<Item> itemsToRemove = new List<Item>();
        Dictionary<Item, float> itemsToUpdate = new Dictionary<Item, float>();

        // Move each item along the belt
        foreach (KeyValuePair<Item, float> kvp in itemsOnBelt)
        {
            Item item = kvp.Key;
            float distanceTravelled = kvp.Value;

            if (item != null && item.itemState == Item.State.Loose)
            {
                float travelDistance = speed * Time.deltaTime;
                item.transform.position += direction * travelDistance;
                distanceTravelled += travelDistance;

                if (distanceTravelled >= maxTravelDistance)
                {
                    // The item has reached the end of the belt
                    itemsToRemove.Add(item);
                }
                else
                {
                    // Update the distance travelled
                    itemsToUpdate[item] = distanceTravelled;
                }
            }
        }

        // Update items that have not reached the end of the belt
        foreach (KeyValuePair<Item, float> kvp in itemsToUpdate)
        {
            itemsOnBelt[kvp.Key] = kvp.Value;
        }

        // Remove items that have reached the end of the belt
        foreach (Item item in itemsToRemove)
        {
            item.SetHoverOffset(0f);
            itemsOnBelt.Remove(item);
        }
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
        foreach (Item item in itemsOnBelt.Keys)
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
            itemsOnBelt[player.carriedItem] = 0f; // Initialize distance travelled
            player.carriedItem.SetHoverOffset(transform.position.y + hoverHeight);
            player.carriedItem = null;
            return;
        }

        // If the player doesn't have an item, take the closest one from the belt
        Item closestItem = null;
        float closestDistance = maxItemDistance;
        foreach (Item item in itemsOnBelt.Keys)
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
            player.carriedItem.SetHoverOffset(0f);
            itemsOnBelt.Remove(closestItem);
        }
    }
}
