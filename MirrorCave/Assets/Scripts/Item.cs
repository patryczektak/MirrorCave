using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    public enum State { Loose, Picked, Locked}
    public State itemState = State.Loose;
    public ItemType type;
    public override bool CanInteract(PlayerController player)
    {
        return player.carriedItem == null && itemState == State.Loose;
    }

    public override void Interact(PlayerController player)
    {
        if (itemState == State.Loose && player.carriedItem == null)
        {
            itemState = State.Picked;
            player.carriedItem = this;
            return;
        }
    }


}
