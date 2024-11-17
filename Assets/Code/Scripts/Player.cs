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

    [Range(0.1f, 1)]
    public float mouseSensitivity = 0.2f;

    public InputAction moveAction;
    public InputAction lookAction;

    private MouseManager mouseManager_;

    public InputActionMap gameplayActions;

    private Rigidbody rb_;
    private GameObject head_;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb_ = GetComponent<Rigidbody>();
        Assert.IsNotNull(this.rb_, "Unable to find Rigidbody on a Player instance.");

        // Find the head using the head's transform
        Transform headTransform = this.transform.Find("Head");
        Assert.IsNotNull(headTransform, "Unable to find Head transform from Player.");
        this.head_ = headTransform.gameObject;
        Assert.IsNotNull(this.head_, "Unable to find Head from head transform.");

        this.mouseManager_ = FindFirstObjectByType<MouseManager>();
        Assert.IsNotNull(this.mouseManager_, "Unable to find the MouseManager from the Player.");

        moveAction.Enable();
        lookAction.Enable();
    }

    // Update every frame
    void Update()
    {
        this.UpdateCamera();

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            mouseManager_.Toggle();
        }
    }

    // Update on a fixed timer
    void FixedUpdate()
    {
        this.UpdatePosition();
    }

    const float EULER_MAX = 90;
    const float EULER_MIN = -90;

    private void UpdateCamera()
    {
        Vector2 mouseXY = this.mouseManager_.delta;

        // Use the sensitivity value
        mouseXY *= this.mouseSensitivity;

        // Debug.Log($"mouseXY: {mouseXY}");

        // Rotate horizontal view (no need for bounds checking)
        this.transform.Rotate(new Vector3(0, mouseXY.x, 0));

        // Rotate head view (vertical)
        Transform headTransform = this.head_.transform;

        // Old X angle in [-180, 180]
        float oldXAngle = headTransform.rotation.eulerAngles.x;
        if (oldXAngle >= 180)
        {
            oldXAngle -= 360;
        }

        if (oldXAngle < EULER_MIN || oldXAngle > EULER_MAX)
        {
            Debug.Log(
                $"Player head vertical rotation {oldXAngle} is out of range [{EULER_MIN},{EULER_MAX}]. Resetting to 0."
            );
            this.ResetVerticalLook();
            oldXAngle = 0;
        }

        // Debug.Log($"Old euler x: {headTransform.rotation.eulerAngles.x}   Adjusted: {oldXAngle}");

        float xDiff = -mouseXY.y;

        const float SAFETY = 1;

        var clampedXDiff = Math.Clamp(
            xDiff,
            EULER_MIN - oldXAngle + SAFETY,
            EULER_MAX - oldXAngle - SAFETY
        );

        // Debug.Log($"xDiff: {xDiff}  clampedXDiff: {clampedXDiff}");

        this.head_.transform.Rotate(new Vector3(clampedXDiff, 0, 0));
    }

    private void ResetVerticalLook()
    {
        Vector3 eulers = this.head_.transform.rotation.eulerAngles;
        this.head_.transform.eulerAngles = new Vector3(0, eulers.y, eulers.z);
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
