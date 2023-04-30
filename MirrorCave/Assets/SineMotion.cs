using UnityEngine;
using UnityEngine.UI;

public class SineMotion : MonoBehaviour
{
    public RectTransform target; // The UI element to animate
    public bool scaleX = true;
    public bool scaleY = true;
    public bool rotateZ = true;
    public float frequency = 1f; // The speed of the animation
    public float magnitude = 1f; // The size of the animation

    private Vector3 initialScale;
    private Quaternion initialRotation;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("No target assigned");
            this.enabled = false;
            return;
        }

        initialScale = target.localScale;
        initialRotation = target.localRotation;
    }

    void Update()
    {
        float sineWave = Mathf.Sin(Time.time * frequency) * magnitude;

        if (scaleX || scaleY)
        {
            Vector3 newScale = initialScale;
            if (scaleX)
            {
                newScale.x = initialScale.x + sineWave;
            }
            if (scaleY)
            {
                newScale.y = initialScale.y + sineWave;
            }
            target.localScale = newScale;
        }

        if (rotateZ)
        {
            Quaternion newRotation = initialRotation * Quaternion.Euler(0, 0, sineWave);
            target.localRotation = newRotation;
        }
    }
}
