using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    public enum State { Loose, Picked, Locked }
    public State itemState = State.Loose;
    public ItemType type;

    private float defaultYValue = 1.0f; // The default Y value
    private float floatSpeed = 5.0f; // The speed at which the item floats
    private float floatAmplitude = 0.1f; // The amplitude of the floating motion

    private float floatOffset = 0.0f; // The current offset from the default Y value
    private float hoveroffset = 0.0f; // The current offset from the default Y value

    [SerializeField]
    private float rotationSpeed = 20.0f; // The speed of rotation

    private void FixedUpdate()
    {
        if (itemState == State.Loose)
        {
            // Move the item towards the default Y value
            Vector3 position = transform.position;
            position.y = Mathf.MoveTowards(position.y, defaultYValue + hoveroffset + floatOffset, floatSpeed * Time.fixedDeltaTime);
            transform.position = position;

            // Update the floating offset
            floatOffset = floatAmplitude * Mathf.Sin(Time.time * floatSpeed);

            // Rotate the item
            transform.Rotate(Vector3.up, rotationSpeed * Time.fixedDeltaTime);

        }
        interactActionName = type.interactName;

    }

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

    public void SetHoverOffset(float offset)
    {
        hoveroffset = offset;
    }
}
