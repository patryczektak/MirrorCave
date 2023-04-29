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
            float distance = Vector3.Distance(player.transform.position, interactable.transform.position);
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
        interactables = new();
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
