using System;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IControllable
{
    public float maxMoveSpeed = 1;
    public float acceleration = 1;

    [Range(0f, 1.0f)]
    public float movementDeadzone = 0.2f;

    public InputAction moveAction;
    private InputAction useAction_;
    private InputAction diskAction_;

    public Transform GetHeadTransform()
    {
        return this.head_.transform;
    }

    // Private


    public InputActionMap gameplayActions;

    private SceneManager mouseManager_;

    private IUsable usable_;
    private Rigidbody rb_;
    public GameObject head_;
    public PlayerReacher reacher_;

    public PlayerInventory inventory;

    private bool currentlyControlling_ = false;

    public void SetControllable(bool enable = true)
    {
        this.currentlyControlling_ = enable;
        if (moveAction == null || useAction_ == null || diskAction_ == null)
        {
            return;
        }

        if (currentlyControlling_)
        {
            moveAction.Enable();
            useAction_.Enable();
            diskAction_.Enable();
        }
        else
        {
            moveAction.Disable();
            useAction_.Disable();
            diskAction_.Disable();
        }
    }

    public bool GetControllable()
    {
        return this.currentlyControlling_;
    }

    public GameObject startAt3dLevel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb_ = GetComponent<Rigidbody>();
        Assert.IsNotNull(this.rb_, "Unable to find Rigidbody on a Player instance.");

        // Find the head using the head's transform
        Transform headTransform = this.transform.Find("Head");
        Assert.IsNotNull(headTransform, "Unable to find Head transform from Player.");
        Assert.IsNotNull(this.head_, "Did not set head_ in the inspector.");

        this.reacher_ = GetComponentInChildren<PlayerReacher>();
        Assert.IsNotNull(headTransform, "Unable to find Reacher transform from Player.");

        this.mouseManager_ = FindFirstObjectByType<SceneManager>();
        Assert.IsNotNull(this.mouseManager_, "Unable to find the MouseManager from the Player.");

        // gameplayActions.Enable();

        inventory = GetComponent<PlayerInventory>();
        Assert.IsNotNull(inventory, "Unable to find PlayerInventory from Player.");

        useAction_ = gameplayActions.FindAction("Use");
        Assert.IsNotNull(useAction_, "Unable to find Use action from Player.");
        diskAction_ = gameplayActions.FindAction("Disk");
        Assert.IsNotNull(diskAction_, "Unable to find Insert action from Player.");

        ContinueGame();

        SetControllable(true);
        useAction_.performed += context => this.reacher_.UseUsable();

        diskAction_.performed += context => this.reacher_.UseDiskAction();
    }

    // Update every frame

    void ContinueGame()
    {
        var continueLevel = PlayerPrefs.GetInt("ContinueLevel");
        var levels2d = FindObjectsByType<Level2D>(FindObjectsSortMode.None).ToList();
        var level2d = levels2d.Find(level => level.levelOrder == continueLevel - 1);
        var computer = level2d == null ? null : level2d.outBrokenComputer;

        if (startAt3dLevel != null || continueLevel != 0)
        {
            if (startAt3dLevel != null)
            {
                computer = startAt3dLevel.GetComponentInChildren<Computer>();
                Debug.LogWarning(
                    $"Manually setting player position to {startAt3dLevel}, please ensure it is removed after you're done!"
                );
            }

            if (computer == null)
            {
                Debug.LogError("Unable to find computer to start at.");
                return;
            }

            var transform = computer.transform.Find("Watcher")?.gameObject.transform;
            this.transform.position = transform.position;
            rb_.position = transform.position;
            if (startAt3dLevel == null)
            {
                computer.state_ = Computer.UseState.Broken;
            }
        }
    }

    void Update()
    {
        if (this.currentlyControlling_)
        {
            this.UpdateCamera();
        }

        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            mouseManager_.ToggleMouseLock();
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
        Vector2 mouseXY = this.mouseManager_.GetScaledDelta();

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

        float xDiff = -mouseXY.y;

        const float SAFETY = 1;

        var clampedXDiff = Math.Clamp(
            xDiff,
            EULER_MIN - oldXAngle + SAFETY,
            EULER_MAX - oldXAngle - SAFETY
        );

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

        // Add movement relative to camera/character orientation
        direction += inputDirections.y * this.transform.forward;
        direction += inputDirections.x * this.head_.transform.right;

        // Add force if high enough
        if (direction.magnitude >= movementDeadzone)
        {
            var force = acceleration * direction.normalized;
            rb_.AddForce(force * Time.deltaTime, ForceMode.VelocityChange);
        }

        var currentVelocity2D = new Vector2(rb_.linearVelocity.x, rb_.linearVelocity.z);
        if (currentVelocity2D.magnitude > maxMoveSpeed)
        {
            var cappedVelocity = currentVelocity2D.normalized * maxMoveSpeed;
            rb_.linearVelocity = new Vector3(
                cappedVelocity.x,
                rb_.linearVelocity.y,
                cappedVelocity.y
            );
        }
    }
}
