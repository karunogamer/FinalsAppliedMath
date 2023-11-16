using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMathScript : MonoBehaviour
{
    public float jumpHeight = 2.5f;
    public float jumpDuration = 0.5f;
    public float gravityLerpTime = 0.1f;

    private float initialGravity;
    private float verticalVelocity = 0f;

    

    void Start()
    {
        initialGravity = gravityLerpTime;
    }

    void Update()
    {
        // Handle Jumping
        if (Input.GetMouseButton(0))
        {
            Jump();
        }

        // Apply gravity using Mathf.Lerp
        verticalVelocity = Mathf.Lerp(verticalVelocity, -initialGravity, gravityLerpTime * Time.deltaTime);

        // Move the bird
        transform.position += new Vector3(0, verticalVelocity * Time.deltaTime, 0);
    }

    void Jump()
    {
        // Calculate jump velocity using a simple math equation
        float jumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(initialGravity) * jumpHeight);

        // Adjust gravity for smooth jumping
        gravityLerpTime = 0.1f;

        // Set the vertical velocity for the jump
        verticalVelocity = jumpVelocity;
    }
}
