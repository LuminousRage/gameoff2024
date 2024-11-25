using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

// Note: I think this is more of a 3D manager, rather than a manager for the whole scene (which is 2D + 3D)
public class SceneManager : MonoBehaviour
{
    public Vector2 mouseDelta { get; private set; } = Vector2.zero;

    [Range(1f, 10f)]
    public float mouseSensitivity = 5f;

    private bool locked = false;

    private GameObject uiCanvas_;

    private TMP_Text useKeyPreview_,
        useText_;

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

    public void SetUsePrompt(string key = "E")
    {
        if (player_.GetControllable())
        {
            var usablePriority = player_.reacher_.SortUsablePriority();
            var mostPriority = usablePriority.FirstOrDefault();
            if (mostPriority != null)
            {
                useText_.text = $"{mostPriority.GetActionLabel()} {mostPriority.GetUsableLabel()}";
                useText_.gameObject.SetActive(true);
                useKeyPreview_.text = key;
                useKeyPreview_.gameObject.SetActive(true);
                return;
            }
        }

        useText_.gameObject.SetActive(false);
        useKeyPreview_.gameObject.SetActive(false);
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

        ValidateFloppyDisks();
    }

    // Update is called once per frame
    void Update()
    {
        SetUsePrompt();
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
        player_.transform.forward = transform.forward;

        var newPosition = new Vector3(
            transform.position.x,
            player_.transform.position.y,
            transform.position.z
        );
        // subtract some offset so the player doesn't appear on the table
        // will need to be adjusted if the table size changes
        player_.transform.position = newPosition + new Vector3(-0.5f, 0, 0);
        followCamera_._followee = transform;
    }

    void ValidateFloppyDisks()
    {
        var floppyDisks = GetComponentsInChildren<FloppyDisk>();
        var distinctFloppyDiskCount = floppyDisks.Select(fd => fd.floppyDiskID).Distinct().Count();

        if (distinctFloppyDiskCount != floppyDisks.Length)
        {
            Debug.LogError("There are duplicate floppy disks in the scene.");
        }
    }
}
