using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smelter : Interactable
{

    public ItemType input;
    public ItemType output;
    private Item contents;
    public float processingTime = 2f, processingTimer;
    public bool processing = false;
    public bool processFinished = false;

    public override bool CanInteract(PlayerController player)
    {
        if (!processing && player.carriedItem != null)
            if (player.carriedItem.type == input) return true;
        return (player.carriedItem == null && processFinished);
    }

    public override void Interact(PlayerController player)
    {
        if (!processing && player.carriedItem != null)
            if (player.carriedItem.type == input)
        {
            contents = player.carriedItem;
            player.carriedItem = null;
            contents.itemState = Item.State.Locked;
            contents.transform.position = transform.position;
            processing = true;
            processingTimer = processingTime;
            return;
        }
        if (player.carriedItem == null && processFinished)
        {
            contents.type = output;
            contents.itemState = Item.State.Picked;
            player.carriedItem = contents;
            contents = null;
            processFinished = false;
            processing = false;
            return;
        }
    }

    private void Update()
    {
        if (processing)
        {
            processingTimer -= Time.deltaTime;
            if (processingTimer < 0) processFinished = true;
        }
    }
}
