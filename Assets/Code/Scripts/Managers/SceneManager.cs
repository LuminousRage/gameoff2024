using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

// Note: I think this is more of a 3D manager, rather than a manager for the whole scene (which is 2D + 3D)
public class SceneManager : MonoBehaviour
{
    public Vector2 mouseDelta { get; private set; } = Vector2.zero;

    private bool locked = false;

    private GameObject uiCanvas_;

    private TMP_Text useKeyPreview_,
        useText_,
        diskText_,
        diskKeyPreview_;

    private Player player_;

    private FollowCamera followCamera_;

    private Computer focusedComputer_ = null;

    public Vector2 GetScaledDelta()
    {
        // Use the sensitivity value
        var sensitivity = PlayerPrefs.GetFloat("SavedSensitivty",0.5f);

        return this.mouseDelta * 10 * sensitivity;
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
            // Maybe I'll clean this up later...
            // Use command
            var usablePriority = player_.reacher_.SortUsablePriority();
            var mostPriority = usablePriority.FirstOrDefault();
            if (mostPriority != null)
            {
                useText_.text = $"{mostPriority.GetActionLabel()} {mostPriority.GetUsableLabel()}";
                useText_.gameObject.SetActive(true);
                useKeyPreview_.text = mostPriority.GetKeyLabel();
                useKeyPreview_.gameObject.SetActive(true);
            }
            else
            {
                useText_.gameObject.SetActive(false);
                useKeyPreview_.gameObject.SetActive(false);
            }

            // Disk command
            var diskActionPrompt = player_.reacher_.GetDiskActionPrompt();
            var (diskKey, diskLabel) = player_.reacher_.DiskActionPromptToString(diskActionPrompt);
            if (diskLabel != null)
            {
                diskText_.text = diskLabel;
                diskText_.gameObject.SetActive(true);
            }
            else
            {
                diskText_.gameObject.SetActive(false);
            }
            if (diskKey != null)
            {
                diskKeyPreview_.text = diskKey;
                diskKeyPreview_.gameObject.SetActive(true);
            }
            else
            {
                diskKeyPreview_.gameObject.SetActive(false);
            }
            return;
        }

        UnsetUsePrompt();
    }

    public void UnsetUsePrompt()
    {
        useText_.gameObject.SetActive(false);
        useKeyPreview_.gameObject.SetActive(false);
        diskText_.gameObject.SetActive(false);
        diskKeyPreview_.gameObject.SetActive(false);
    }

    public void SetFocus(Computer computer)
    {
        if (computer)
        {
            followCamera_._followee = computer.GetWatcherTransform();
            player_.SetControllable(false);

            computer.level.EnterFrom(computer);
            player_.inventory.PutDownDisks(computer);

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

        diskText_ = uiCanvas_.transform.Find("DiskText").GetComponent<TMP_Text>();
        Assert.IsNotNull(diskText_);

        diskKeyPreview_ = uiCanvas_.transform.Find("DiskKeyPreview").GetComponent<TMP_Text>();
        Assert.IsNotNull(diskKeyPreview_);

        this.followCamera_ = FindFirstObjectByType<FollowCamera>();
        Assert.IsNotNull(followCamera_, "Unable to find FollowCamera from the scene.");

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
