using UnityEngine;

using UnityEngine;

public class HamsterWheel : Interactable
{
    [SerializeField] private float charge = 0.0f;
    [SerializeField] private float maxCharge = 3f;
    [SerializeField] private Vector3 correctDirection = Vector3.forward;
    [SerializeField] private Transform lockedPlayerPosition;
    [SerializeField] private Transform spawnItemPosition;
    [SerializeField] private GameObject itemPrefab; // New field for the item prefab

    [SerializeField] private Transform wheelTransform, drillTransform;
    private PlayerController lockedPlayer;

    private void FixedUpdate()
    {
        if (lockedPlayer != null)
        {
            charge += Vector3.Dot(lockedPlayer.rb.velocity.normalized, correctDirection.normalized) * Time.fixedDeltaTime;
            charge = Mathf.Max(0, charge); // Ensure charge doesn't go below 0

            wheelTransform.localEulerAngles = new Vector3(charge * 360f, 0f, 90f);
            
            Vector3 drillPos = drillTransform.localPosition;
            drillPos.y = 4f - charge;
            drillTransform.localPosition = drillPos;

            if (charge >= maxCharge)
            {
                // Reset the charge
                charge = 0;

                // Spawn the item at the desired position
                Instantiate(itemPrefab, spawnItemPosition.position, Quaternion.identity);
            }
        }
        drillTransform.localEulerAngles = new Vector3(0f, Time.time * 720f, 0f);
    }

public override bool CanInteract(PlayerController player)
    {
        // The player can interact with the wheel if it is not currently in use
        if (lockedPlayer == null)
        {
            interactActionName = "Use Wheel";
            return true;
        }

        // If the player using the wheel tries to interact, they will stop using it
        if (player == lockedPlayer)
        {
            interactActionName = "Stop Using Wheel";
            return true;
        }

        return false;
    }

    public override void Interact(PlayerController player)
    {
        if (player == lockedPlayer)
        {
            // If the interacting player is already using the wheel, release them
            player.lockedPosition = lockedPlayerPosition.position; // Set the locked position before unlocking
            player.transform.position = spawnItemPosition.position;
            player.Locked = false;
            lockedPlayer = null;
        }
        else if (lockedPlayer == null)
        {
            // If no one is using the wheel, lock the interacting player to it
            player.lockedPosition = lockedPlayerPosition.position; // Set the locked position before locking
            player.Locked = true;
            lockedPlayer = player;
        }
    }
}

