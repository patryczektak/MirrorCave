using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public float interactDistance = 2f;
    public string interactActionName = "";


    public virtual bool CanInteract(PlayerController player)
    {
        Debug.LogWarning("No Can Interact Override");
        return false;
    }

    public virtual void Interact( PlayerController player)
    {
        Debug.LogWarning("No Interact Override");
    }

    public virtual void Start()
    {
        EntityManager.Register(this);
    }

    private void OnDestroy()
    {
        EntityManager.Unregister(this);
    }
}
