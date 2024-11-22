using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class SceneManager : MonoBehaviour
{
    public Vector2 mouseDelta { get; private set; } = Vector2.zero;

    [Range(1f, 10f)]
    public float mouseSensitivity = 5f;

    private bool locked = false;

    private GameObject uiCanvas_;

    private TMP_Text useText_;
    private TMP_Text useKeyPreview_;

    private Player player_;

    private FollowCamera followCamera_;

    private Computer focusedComputer_ = null;

    private ComputerManager computerManager_;

    public Vector2 GetScaledDelta()
    {
        // Use the sensitivity value
        return this.mouseDelta * this.mouseSensitivity;
    }

    public void ToggleMouseLock()
    {
        if (this.locked)
            this.UnlockMouse();
        else
            this.LockMouse();
    }

    public void LockMouse()
    {
        if (this.locked)
            Debug.LogWarning($"Attempted to lock the MouseManager when it it already locked.");
        else
        {
            Debug.Log("Locking the cursor.");
        }

        Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
        Cursor.visible = true;

        this.locked = true;

        return;
    }

    public void UnlockMouse()
    {
        if (!this.locked)
            Debug.LogWarning($"Attempted to unlock the MouseManager when it it already unlocked.");
        else
        {
            Debug.Log("Unlocking the cursor.");
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        this.locked = false;
    }

    public void SetUsePrompt(IUsable usable)
    {
        useText_.text = $"Use {usable.GetUsableLabel()}";
        useText_.gameObject.SetActive(true);
        useKeyPreview_.gameObject.SetActive(true);
    }

    public void UnsetUsePrompt()
    {
        useText_.gameObject.SetActive(false);
        useKeyPreview_.gameObject.SetActive(false);
    }

    public void SetFocus(Computer computer)
    {
        if (computer)
        {
            followCamera_._followee = computer.GetWatcherTransform();
            player_.SetControllable(false);

            computer.level.EnterFrom(computer);

            this.UnsetUsePrompt();
        }
        else
        {
            followCamera_._followee = player_.GetHeadTransform();
            player_.SetControllable(true);
        }

        focusedComputer_?.level.Exit();
        focusedComputer_ = computer;
    }

    void Start()
    {
        this.uiCanvas_ = this.transform.Find("UICanvas")?.gameObject;
        Assert.IsNotNull(this.uiCanvas_);

        this.player_ = GameObject.FindFirstObjectByType<Player>();
        Assert.IsNotNull(this.player_);

        this.useText_ = this.uiCanvas_.transform.Find("UsableText").GetComponent<TMP_Text>();
        Assert.IsNotNull(this.useText_);

        useKeyPreview_ = uiCanvas_.transform.Find("KeyPreview").GetComponent<TMP_Text>();
        Assert.IsNotNull(useKeyPreview_);

        this.followCamera_ = FindFirstObjectByType<FollowCamera>();
        Assert.IsNotNull(followCamera_, "Unable to find FollowCamera from the scene.");

        computerManager_ = GetComponent<ComputerManager>();

        this.LockMouse();
        this.UnsetUsePrompt();

        // Start the game with the 3D player as controllable
        this.SetFocus(null);
    }

    // Update is called once per frame
    void Update()
    {
        this.UpdateMouseDelta();
        this.UpdateUICanvas();
    }

    private void UpdateMouseDelta()
    {
        var newDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        this.mouseDelta = newDelta;
    }

    private void UpdateUICanvas()
    {
        // var headTransform = player_.GetHeadTransform();

        // this.uiCanvas_.transform.position = headTransform.position + headTransform.forward * 2;
        // this.uiCanvas_.transform.rotation = headTransform.rotation;
    }

    public void UpdatePlayerLocation(Transform transform)
    {
        player_.transform.position = transform.position;
        followCamera_._followee = transform;
    }
}
