using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float maxMoveSpeed = 1;
    public float acceleration = 1;

    [Range(0f, 1.0f)]
    public float movementDeadzone = 0.2f;

    public InputAction moveAction;
    public InputAction lookAction;

    public InputActionMap gameplayActions;

    private Rigidbody rb_;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb_ = GetComponent<Rigidbody>();
        Assert.IsNotNull(rb_, "Unable to find Rigidbody on a Player instance.");

        moveAction.Enable();
        lookAction.Enable();
    }

    // Update is called once per frame
    void Update() { }

    void FixedUpdate()
    {
        var directions = moveAction.ReadValue<Vector2>();

        Vector3 direction = Vector3.zero;
        direction += directions.x * Vector3.right;
        direction += directions.y * Vector3.forward;

        // Add force if high enough
        if (direction.magnitude >= movementDeadzone)
        {
            var force = acceleration * direction.normalized;
            Debug.Log($"Force: {force}");

            // rb_.AddForce(acceleration * direction.normalized, ForceMode.Acceleration);
            rb_.AddForce(force);
        }

        float currentSpeed = rb_.linearVelocity.magnitude;

        if (currentSpeed > maxMoveSpeed)
        {
            rb_.linearVelocity = rb_.linearVelocity.normalized * maxMoveSpeed;
        }
    }
}
