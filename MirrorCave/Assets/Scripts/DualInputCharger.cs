using UnityEngine;

public class DualInputCharger : Interactable
{
    [SerializeField] private float charge = 0.0f;
    [SerializeField] private float maxCharge = 3f;  // 3 full rotations
    [SerializeField] private ItemType requiredItemType1;
    [SerializeField] private ItemType requiredItemType2;
    [SerializeField] private GameObject newItemPrefab = null;
    [SerializeField] private Transform newItemSpawnPoint = null;
    [SerializeField] private float fixedDistance = 2f;
    private PlayerController chargingPlayer = null;
    private Item item1 = null;
    private Item item2 = null;
    private float lastAngle = 0f;

    private void FixedUpdate()
    {
        if (chargingPlayer != null)
        {
            // Calculate the direction to the player
            Vector3 directionToPlayer = chargingPlayer.transform.position - transform.position;

            // Enforce the player to stay at a fixed distance from the center
            if (directionToPlayer.magnitude != fixedDistance)
            {
                chargingPlayer.transform.position = transform.position + directionToPlayer.normalized * fixedDistance;
                directionToPlayer = chargingPlayer.transform.position - transform.position;
            }

            // Calculate the current angle
            float currentAngle = Vector3.SignedAngle(Vector3.right, directionToPlayer, Vector3.up);

            // Calculate the change in angle from the last frame
            float deltaAngle = Mathf.DeltaAngle(lastAngle, currentAngle);

            // If the change in angle is positive, the player is moving clockwise, so increase the charge
            charge += deltaAngle / 360f; // divide by 360 to convert to rotations

            // If the charge reaches or exceeds the max charge, call the ChargeComplete method
            if (charge >= maxCharge) ChargeComplete();

            // Update the last angle for the next frame
            lastAngle = currentAngle;
        }
    }


    public override bool CanInteract(PlayerController player)
    {
        if (player.carriedItem != null)
        {
            if ((item1 == null && player.carriedItem.type == requiredItemType1) ||
                (item2 == null && player.carriedItem.type == requiredItemType2))
            {
                interactActionName = "Insert Item";
                return true;
            }
        }

        if (item1 != null && item2 != null && chargingPlayer == null)
        {
            interactActionName = "Start Charging";
            return true;
        }

        if (chargingPlayer == player)
        {
            interactActionName = "Stop Charging";
            return true;
        }

        return false;
    }

    private void ChargeComplete()
    {
        // Destroy the items and reset the item references
        Destroy(item1.gameObject);
        Destroy(item2.gameObject);
        item1 = null;
        item2 = null;

        // Instantiate the new item at the specified spawn point
        Instantiate(newItemPrefab, newItemSpawnPoint.position, Quaternion.identity);

        // Reset the charge
        charge = 0f;
        Interact(chargingPlayer);
    }

    public override void Interact(PlayerController player)
    {
        // If the player interacts while they are charging, stop the charging process
        if (chargingPlayer == player)
        {
            chargingPlayer = null;
            lastAngle = 0f;
        }
        else if (item1 != null && item2 != null && chargingPlayer == null)
        {
            // If both items are present and no one is currently charging, start the charging process
            chargingPlayer = player;
            Vector3 directionToPlayer = chargingPlayer.transform.position - transform.position;
            lastAngle = Vector3.SignedAngle(Vector3.right, directionToPlayer, Vector3.up);
        }
        else if (player.carriedItem != null && (item1 == null || item2 == null))
        {
            // If the player is carrying an item, and the charger doesn't have both items yet, accept the item
            if (player.carriedItem.type == requiredItemType1 && item1 == null)
            {
                item1 = player.carriedItem;
                item1.transform.position = transform.position;  // move the item to the center of the charger
                player.carriedItem = null;
            }
            else if (player.carriedItem.type == requiredItemType2 && item2 == null)
            {
                item2 = player.carriedItem;
                item2.transform.position = transform.position;  // move the item to the center of the charger
                player.carriedItem = null;
            }
        }
    }
}
