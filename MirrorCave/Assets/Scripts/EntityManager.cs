using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    private static EntityManager Instance;

    private List<Interactable> interactables;

    public static Interactable GetInteractable(PlayerController player)
    {
        Interactable closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Interactable interactable in Instance.interactables)
        {
            Vector3 playerPosition = player.transform.position;
            Vector3 interactablePosition = interactable.transform.position;

            // Calculate 2D distance by ignoring the Y axis
            float distance = Vector2.Distance(new Vector2(playerPosition.x, playerPosition.z), new Vector2(interactablePosition.x, interactablePosition.z));

            if (distance < interactable.interactDistance && distance < closestDistance && interactable.CanInteract(player))
            {
                closest = interactable;
                closestDistance = distance;
            }
        }

        return closest;
    }


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        interactables = new List<Interactable>();
    }



    internal static void Register(Interactable interactable)
    {
        Instance.interactables.Add(interactable);
    }

    internal static void Unregister(Interactable interactable)
    {
        Instance.interactables.Remove(interactable);
    }
}
