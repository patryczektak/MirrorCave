using UnityEngine;

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
    [SerializeField] private bool mirrorX = false;
    [SerializeField] private bool mirrorZ = false;
    [SerializeField] private float iceEffect = 0.95f; // Determines how long the ice effect lasts. The closer to 1, the longer it lasts

    private Vector3 movement;

    private void Start()
    {
        KeyBindingsManager.ContinuousAction("move left", MoveLeft);
        KeyBindingsManager.ContinuousAction("move right", MoveRight);
        KeyBindingsManager.ContinuousAction("move forward", MoveForward);
        KeyBindingsManager.ContinuousAction("move backward", MoveBackward);
    }

    private void FixedUpdate()
    {
        if (status == PlayerStatus.Active)
        {
            transform.position += movement * speed * Time.deltaTime;
            movement *= iceEffect; // Apply ice effect by reducing the movement vector over time
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
}
