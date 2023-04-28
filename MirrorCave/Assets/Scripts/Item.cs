using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    public enum state { Loose, Picked, Locked}
    public state itemState = state.Loose;
    public override bool CanInteract(PlayerController player)
    {
        return player.carriedItem == null && itemState == state.Loose;
    }

    public override void Interact(PlayerController player)
    {
        if (itemState == state.Loose && player.carriedItem == null)
        {
            itemState = state.Picked;
            player.carriedItem = this;
            return;
        }
    }


}
