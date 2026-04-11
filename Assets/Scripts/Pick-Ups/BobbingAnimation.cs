using System.Numerics;
using UnityEngine;

public class BobbingAnimation : MonoBehaviour
{
    public float frequency; // speed of bobbing movement
    public float magnitude; // range of bobbing movement from initial location
    public UnityEngine.Vector3 direction;
    UnityEngine.Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        // Sine function for smooth bobbing effect
        transform.position = initialPosition + direction * Mathf.Sin(Time.time * frequency) * magnitude;
    }
}
