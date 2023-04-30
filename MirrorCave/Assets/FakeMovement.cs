using UnityEngine;

public class FakeMovement : MonoBehaviour
{
    [SerializeField] private bool rotateX = false;
    [SerializeField] private bool rotateY = true;
    [SerializeField] private bool rotateZ = true;
    [SerializeField] private float degreeLimit = 1f;
    [SerializeField] private float speed = 60f;

    private Vector3 initialRotation;

    void Start()
    {
        // Save the initial rotation
        initialRotation = transform.rotation.eulerAngles;
    }

    void Update()
    {
        // Compute the new rotation based on the sine wave
        float sinValue = Mathf.Sin(Time.time * speed) * degreeLimit;

        Vector3 newRotation = new Vector3(
            rotateX ? initialRotation.x + sinValue : initialRotation.x,
            rotateY ? initialRotation.y + sinValue : initialRotation.y,
            rotateZ ? initialRotation.z + sinValue : initialRotation.z
        );

        // Apply the new rotation to the object
        transform.rotation = Quaternion.Euler(newRotation);
    }
}
