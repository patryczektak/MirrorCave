using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropoff : Interactable
{

    [SerializeField] ItemType itemType;
    [SerializeField] float pointsPerItem;

    public override bool CanInteract(PlayerController player)
    {
        if (player.carriedItem == null) return false;
        if (player.carriedItem.type == itemType) return true;
        return false;
    }

    public override void Interact(PlayerController player)
    {
        Destroy(player.carriedItem.gameObject);
        player.carriedItem = null;
        GameManager.AddScore(pointsPerItem);
    }
}
