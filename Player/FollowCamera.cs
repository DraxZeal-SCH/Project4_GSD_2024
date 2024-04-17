// This script makes the camera follow a target smoothly.
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // The target the camera will follow
    public Transform target;

    // The speed at which the camera follows the target
    public float smoothSpeed = 0.125f;

    // The offset from the target's position
    public Vector3 offset;

    void FixedUpdate()
    {
        if (target != null)
        {
            // Calculate the desired position for the camera
            Vector3 desiredPosition = target.position + offset;

            // Smoothly move the camera towards the desired position using Lerp
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Set the camera's position to the smoothed position
            transform.position = smoothedPosition;
        }
    }
}
