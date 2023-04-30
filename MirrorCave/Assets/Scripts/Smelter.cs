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

    [SerializeField] private GameObject particleObject;

    public override void Start()
    {
        base.Start();
        particleObject.SetActive(false);
    }

    public override bool CanInteract(PlayerController player)
    {
        if (!processing && player.carriedItem != null)
            if (player.carriedItem.type == input)
            {
                interactActionName = "Smelt Ore";
                return true;
            }
        if (player.carriedItem == null && processFinished)
        {
            interactActionName = "Take Out Bar";
            return true;
        }
        return false;
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
            particleObject.SetActive(true);
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
            if (processingTimer < 0)
            {
                processFinished = true;
                particleObject.SetActive(false);
            }
        }
    }
}
