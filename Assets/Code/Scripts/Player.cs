using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IControllable
{
    public float maxMoveSpeed = 1;
    public float acceleration = 1;

    [Range(0f, 1.0f)]
    public float movementDeadzone = 0.2f;

    [
        Range(1.2f, 3f),
        Tooltip(
            "The ratio to apply when the player sprints. For example, at 2.0f the player will run twice as fast when sprinting."
        )
    ]
    public float sprintRatio = 1.6f;

    public InputAction moveAction;
    private InputAction useAction_;
    private InputAction diskAction_;
    private InputAction sprintAction_;
    private InputAction pauseAction_;

    public Transform GetHeadTransform()
    {
        return this.head_.transform;
    }

    public InputActionMap gameplayActions;

    private SceneManager sceneManager_;

    private IUsable usable_;

    [HideInInspector]
    public Rigidbody rb_;
    public GameObject head_;
    public PlayerReacher reacher_;

    public PlayerInventory inventory;

    public int levelOverride = 0;

    private bool currentlyControlling_ = false;
    private bool currentlySprinting_ = false;

    private Nullable<float> cancelPauseIfTimeIsOver_ = null;

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
            sprintAction_.Enable();
            pauseAction_.Enable();
        }
        else
        {
            moveAction.Disable();
            useAction_.Disable();
            diskAction_.Disable();
            sprintAction_.Disable();
        }
    }

    public bool GetControllable()
    {
        return this.currentlyControlling_;
    }

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

        this.sceneManager_ = FindFirstObjectByType<SceneManager>();
        Assert.IsNotNull(this.sceneManager_, "Unable to find the MouseManager from the Player.");

        // gameplayActions.Enable();

        inventory = GetComponent<PlayerInventory>();
        Assert.IsNotNull(inventory, "Unable to find PlayerInventory from Player.");

        useAction_ = gameplayActions.FindAction("Use");
        Assert.IsNotNull(useAction_, "Unable to find Use action from Player.");
        diskAction_ = gameplayActions.FindAction("Disk");
        Assert.IsNotNull(diskAction_, "Unable to find Insert action from Player.");
        sprintAction_ = gameplayActions.FindAction("Sprint");
        Assert.IsNotNull(sprintAction_, "Unable to find Sprint action from Player.");
        pauseAction_ = gameplayActions.FindAction("Pause");
        Assert.IsNotNull(pauseAction_, "Unable to find Pause action from Player.");

        SetControllable(true);
        useAction_.performed += context =>
            sceneManager_.RunActionWithInputLock(() => this.reacher_.UseUsable(), context.action);

        diskAction_.performed += context => this.reacher_.UseDiskAction();

        sprintAction_.started += context => this.currentlySprinting_ = true;
        sprintAction_.canceled += context => this.currentlySprinting_ = false;

        pauseAction_.performed += context =>
        {
            if (this.cancelPauseIfTimeIsOver_ != null)
            {
                sceneManager_.UnlockMouse();
                UnityEngine.SceneManagement.SceneManager.LoadScene("Main menu");
                return;
            }

            this.cancelPauseIfTimeIsOver_ = UnityEngine.Time.time + 3f;
            sceneManager_.SetExitTextActive(true);
        };
    }

    void Update()
    {
        if (this.currentlyControlling_)
        {
            this.UpdateCamera();
        }

        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            sceneManager_.ToggleMouseLock();
        }
    }

    // Update on a fixed timer
    void FixedUpdate()
    {
        // Don't need to check every frame, so its in FixedUpdate
        this.CheckForExit();

        this.UpdatePosition();
    }

    const float EULER_MAX = 90;
    const float EULER_MIN = -90;

    private void UpdateCamera()
    {
        Vector2 mouseXY = this.sceneManager_.GetScaledDelta();

        Transform headTransform = this.head_.transform;

        // Rotate horizontal view (no need for bounds checking)
        headTransform.RotateAround(headTransform.position, Vector3.up, mouseXY.x);

        // headTransform.rotation *= Quaternion.Euler(0, mouseXY.x, 0);

        // Rotate head view (vertical)

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
        direction +=
            inputDirections.y
            * new Vector3(head_.transform.forward.x, 0, head_.transform.forward.z).normalized;
        direction += inputDirections.x * this.head_.transform.right;

        float sprintModifier = currentlySprinting_ ? 2 : 1;

        direction *= sprintModifier;

        // Add force if high enough
        if (direction.magnitude >= movementDeadzone)
        {
            var force = acceleration * direction.normalized;
            rb_.AddForce(force * Time.deltaTime, ForceMode.VelocityChange);
        }

        var currentVelocity2D = new Vector2(rb_.linearVelocity.x, rb_.linearVelocity.z);

        var maxSpeed = GetMaxMoveSpeed();
        if (currentVelocity2D.magnitude > maxSpeed)
        {
            var cappedVelocity = currentVelocity2D.normalized * maxSpeed;
            rb_.linearVelocity = new Vector3(
                cappedVelocity.x,
                rb_.linearVelocity.y,
                cappedVelocity.y
            );
        }
    }

    // Gets the max speed with respect to sprinting
    private float GetMaxMoveSpeed()
    {
        return this.maxMoveSpeed * (this.currentlySprinting_ ? this.sprintRatio : 1);
    }

    public void CheckForExit()
    {
        if (this.cancelPauseIfTimeIsOver_ != null && Time.time >= this.cancelPauseIfTimeIsOver_)
        {
            this.cancelPauseIfTimeIsOver_ = null;
            sceneManager_.SetExitTextActive(false);
        }
    }
}
