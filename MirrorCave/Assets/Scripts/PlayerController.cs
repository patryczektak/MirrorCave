using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour
{
    public enum PlayerStatus
    {
        Active,
        Inactive
    }

    [SerializeField] private PlayerStatus status;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float acceleratrion = 5.0f;
    [SerializeField] public string interactAction = "";
    [SerializeField] private bool mirrorX = false;
    [SerializeField] private bool mirrorZ = false;
    [SerializeField] private float iceEffect = 0.95f; // Determines how long the ice effect lasts. The closer to 1, the longer it lasts

    //Default Y coordinate and some fluff
    private float defaultYValue = 1.5f; // The default Y value
    private float floatSpeed = 5.0f; // The speed at which the item floats



    public Rigidbody rb;

    public Item carriedItem = null;
    public string interactName = "";

    public GameObject stunParticles;

    public Vector3 lockedPosition;
    private bool locked = false;

    private Vector3 movement;

    public bool Locked { get => locked; set => locked = value; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        KeyBindingsManager.ContinuousAction("move left", MoveLeft);
        KeyBindingsManager.ContinuousAction("move right", MoveRight);
        KeyBindingsManager.ContinuousAction("move forward", MoveForward);
        KeyBindingsManager.ContinuousAction("move backward", MoveBackward);
        KeyBindingsManager.Action(interactAction, Interact);
        stunParticles.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (status == PlayerStatus.Active)
        {
            rb.velocity = movement * speed;
            movement *= iceEffect; // Apply ice effect by reducing the movement vector over time

            // Rotate the game object to face the direction of movement if it is moving
            if (movement != Vector3.zero)
            {
                transform.LookAt(transform.position + movement);
            }
        }
        if (carriedItem)
        {
            carriedItem.transform.position = transform.position;
        }
        interactName = GetInteractName();
        Vector3 position = transform.position;
        position.y = Mathf.MoveTowards(position.y, defaultYValue, floatSpeed * Time.fixedDeltaTime);
        transform.position = position;
        if (locked)
        {
            transform.position = lockedPosition;
        }
    }

    public void Stun(float seconds)
    {
        status = PlayerStatus.Inactive;
        StartCoroutine(stunDuration(seconds));
        stunParticles.SetActive(true);
    }
 
    public IEnumerator stunDuration(float secs)
    {
        yield return new WaitForSeconds(secs);
        status = PlayerStatus.Active;
        stunParticles.SetActive(false);
    }

    private string GetInteractName()
    {
        Interactable interactable = EntityManager.GetInteractable(this);
        if (interactable == null)
        {
            if (carriedItem == null)
            {
                return "";
            }
            else
            {
                return "Drop";
            }
        }
        else
        {
            return interactable.interactActionName;
        }
    }

    private void Interact()
    {
        Interactable interactable = EntityManager.GetInteractable(this);
        if (interactable != null)
        {
            interactable.Interact(this);
            return;
        }
        if (carriedItem != null) 
        {
            carriedItem.itemState = Item.State.Loose;
            carriedItem = null;
        }
    }

    private void MoveLeft()
    {
        movement.x += (mirrorX ? acceleratrion : -acceleratrion) * Time.deltaTime;
    }

    private void MoveRight()
    {
        movement.x += (mirrorX ? -acceleratrion : acceleratrion) * Time.deltaTime;
    }

    private void MoveForward()
    {
        movement.z += (mirrorZ ? acceleratrion : -acceleratrion) * Time.deltaTime;
    }

    private void MoveBackward()
    {
        movement.z += (mirrorZ ? -acceleratrion : acceleratrion) * Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6)
        {
            Stun(3);
        }
    }
}
