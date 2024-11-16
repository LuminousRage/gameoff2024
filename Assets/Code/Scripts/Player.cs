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
    private GameObject head_;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb_ = GetComponent<Rigidbody>();
        Assert.IsNotNull(rb_, "Unable to find Rigidbody on a Player instance.");

        Transform headTransform = this.transform.Find("Head");
        Assert.IsNotNull(headTransform, "Unable to find Head transform from Player.");

        this.head_ = headTransform.gameObject;
        Assert.IsNotNull(this.head_, "Unable to find Head from head transform.");

        moveAction.Enable();
        lookAction.Enable();
    }

    // Update every frame
    void Update()
    {
        this.UpdateCamera();
    }

    // Update on a fixed timer
    void FixedUpdate()
    {
        this.UpdatePosition();
    }

    private void UpdateCamera()
    {
        var mouseXY = lookAction.ReadValue<Vector2>();
        this.transform.Rotate(new Vector3(0, mouseXY.x, 0));
        this.head_.transform.Rotate(new Vector3(-mouseXY.y, 0, 0));

        // // Add force if high enough
        // if (direction.magnitude >= movementDeadzone)
        // {
        //     var force = acceleration * direction.normalized;
        //     Debug.Log($"Force: {force}");

        //     // rb_.AddForce(acceleration * direction.normalized, ForceMode.Acceleration);
        //     rb_.AddForce(force);
        // }

        // float currentSpeed = rb_.linearVelocity.magnitude;

        // if (currentSpeed > maxMoveSpeed)
        // {
        //     rb_.linearVelocity = rb_.linearVelocity.normalized * maxMoveSpeed;
        // }
    }

    private void UpdatePosition()
    {
        var inputDirections = moveAction.ReadValue<Vector2>();

        Vector3 direction = Vector3.zero;

        direction += inputDirections.y * this.transform.forward;
        direction += inputDirections.x * this.head_.transform.right;

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
