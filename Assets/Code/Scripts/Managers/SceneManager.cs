using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

// Note: I think this is more of a 3D manager, rather than a manager for the whole scene (which is 2D + 3D)
public class SceneManager : MonoBehaviour
{
    public Vector2 mouseDelta { get; private set; } = Vector2.zero;

    private bool locked = false;

    private GameObject uiCanvas_;

    private TextPrompt diskPrompt_,
        usePrompt_;

    private Player player_;

    [HideInInspector]
    public FollowCamera followCamera_;

    [HideInInspector]
    public Computer focusedComputer_ = null;

    [HideInInspector]
    public bool canUseAction = true;

    private Vector3 initialUsePromptPosition;
    private Vector3 initialDiskPromptPosition;
    public Vector3 usePromptPosition2D = new Vector3(-246, -301.3333f, 0);
    public Vector3 diskPromptPosition2D = new Vector3(149.3333f, -301.3333f, 0);

    [HideInInspector]
    public Avatar avatarActive = null;

    public LevelData levelData;

    private GameObject[] levelsLoaded_ = { };

    private struct TextPrompt
    {
        public GameObject parent;
        public TMP_Text text;
        public TMP_Text keyPreview;

        public void SetActive(bool active)
        {
            text.gameObject.SetActive(active);
            keyPreview.gameObject.SetActive(active);
        }
    }

    private ComputerManager cm_;

    void Start()
    {
        this.uiCanvas_ = this.transform.Find("UICanvas")?.gameObject;
        Assert.IsNotNull(this.uiCanvas_);

        this.player_ = GameObject.FindFirstObjectByType<Player>();
        Assert.IsNotNull(this.player_);

        this.followCamera_ = FindFirstObjectByType<FollowCamera>();
        Assert.IsNotNull(followCamera_, "Unable to find FollowCamera from the scene.");

        cm_ = GetComponent<ComputerManager>();
        Assert.IsNotNull(cm_, "Unable to find ComputerManager from the scene.");

        this.usePrompt_ = PromptObjectToStruct(
            this.uiCanvas_.transform.Find("UsePrompt")?.gameObject
        );
        this.diskPrompt_ = PromptObjectToStruct(
            this.uiCanvas_.transform.Find("DiskPrompt")?.gameObject
        );

        Assert.IsNotNull(
            levelData,
            "No level data given to SceneManager. No levels can be loaded."
        );

        levelsLoaded_ = new GameObject[levelData.levelPrefabs.Length];

        initialDiskPromptPosition = diskPrompt_.parent.transform.localPosition;
        initialUsePromptPosition = usePrompt_.parent.transform.localPosition;

        this.LockMouse();

        // Start the game with the 3D player as controllable
        this.SetFocus(null);

        ValidateFloppyDisks();
        ContinueGame();
    }

    void ContinueGame()
    {
        var continueLevel = PlayerPrefs.GetInt("ContinueLevel");
        EnsureLoaded(continueLevel);
        if (continueLevel > 0)
        {
            var previousLevel = GetLevel(continueLevel - 1);
            var exitComputer = previousLevel.GetComponentInChildren<Level2D>().outBrokenComputer;
            UpdatePlayerLocation(exitComputer.GetWatcherTransform());
            exitComputer.state_ = Computer.UseState.Broken;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetUsePrompt();
        this.UpdateMouseDelta();
    }

    public Vector2 GetScaledDelta()
    {
        // Use the sensitivity value
        var sensitivity = PlayerPrefs.GetFloat("Savedsensitivity", 0.5f);

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

    public void SetUsePrompt()
    {
        string useKeyText = null,
            useText = null,
            diskKeyText = null,
            diskText = null;

        Vector3 usePromptPosition = usePrompt_.parent.transform.position,
            diskPromptPosition = diskPrompt_.parent.transform.position;

        if (player_.GetControllable())
        {
            usePromptPosition = initialUsePromptPosition;
            diskPromptPosition = initialDiskPromptPosition;

            // Use command
            var usablePriority = player_.reacher_.SortUsablePriority();
            var mostPriority = usablePriority.FirstOrDefault();
            useText = mostPriority != null ? mostPriority.GetUsePrompt() : null;
            useKeyText = mostPriority != null ? mostPriority.GetKeyLabel() : null;

            // Disk command
            var diskActionPrompt = player_.reacher_.GetDiskActionPrompt();
            (diskKeyText, diskText) = player_.reacher_.DiskActionPromptToString(diskActionPrompt);
        }
        else if (avatarActive != null)
        {
            usePromptPosition = usePromptPosition2D;
            diskPromptPosition = diskPromptPosition2D;

            var standUpable = avatarActive.az.currentCollisionZone.isStandUpable;
            useText = standUpable ? "Stand up from the computer" : null;
            useKeyText = standUpable ? "E" : null;

            var avatarUsable = avatarActive.ai.usable;
            diskKeyText = avatarUsable != null ? avatarUsable.GetKeyLabel() : null;
            diskText = avatarUsable != null ? avatarUsable.GetUsePrompt() : null;
        }

        SetPromptText(diskPrompt_, diskKeyText, diskText);
        SetPromptText(usePrompt_, useKeyText, useText);

        if (usePrompt_.parent.transform.position != usePromptPosition)
        {
            usePrompt_.parent.transform.localPosition = usePromptPosition;
        }
        if (diskPrompt_.parent.transform.position != diskPromptPosition)
        {
            diskPrompt_.parent.transform.localPosition = diskPromptPosition;
        }
    }

    private void SetPromptText(TextPrompt prompt, string keyText = null, string text = null)
    {
        keyText = keyText ?? "";
        text = text ?? "";

        prompt.keyPreview.text = keyText;
        prompt.text.text = text;
    }

    private TextPrompt PromptObjectToStruct(GameObject promptObject)
    {
        Assert.IsNotNull(promptObject, $"{promptObject.name} is null.");
        var text = promptObject.transform.Find("Text").GetComponent<TMP_Text>();
        var keyPreview = promptObject.transform.Find("KeyPreview").GetComponent<TMP_Text>();

        return new TextPrompt
        {
            parent = promptObject,
            text = text,
            keyPreview = keyPreview,
        };
    }

    public void SetFocus(Computer computer)
    {
        if (computer)
        {
            followCamera_._followee = computer.GetWatcherTransform();
            player_.SetControllable(false);

            computer.level.EnterFrom(computer);
            player_.inventory.PutDownDisks(computer);
            focusedComputer_ = computer;
        }
        else
        {
            followCamera_._followee = player_.GetHeadTransform();
            player_.SetControllable(true);
            focusedComputer_?.level.OnStandUp();
        }
    }

    private void UpdateMouseDelta()
    {
        var newDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        this.mouseDelta = newDelta;
    }

    public void UpdatePlayerLocation(Transform transform)
    {
        // subtract some offset so the player doesn't appear on the table
        // will need to be adjusted if the table size changes

        player_.transform.position =
            transform.position + -0.7f * transform.forward + new Vector3(0, -0.6f, 0);
        player_.GetComponent<Rigidbody>().MovePosition(player_.transform.position);
        player_.transform.forward = new Vector3(
            transform.forward.x,
            0,
            transform.forward.z
        ).normalized;
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

    public IEnumerator ResetUseAction(InputAction action)
    {
        // Wait for a short moment to prevent immediate re-use
        // yield return new WaitForEndOfFrame();

        // Wait until the key is released
        yield return new WaitUntil(() => !action.IsPressed());

        canUseAction = true;
    }

    public void RunActionWithInputLock(Action function, InputAction inputAction)
    {
        if (canUseAction)
        {
            function();
            canUseAction = false;

            StartCoroutine(ResetUseAction(inputAction));
        }
    }

    public void EnsureLoaded(int levelIndex)
    {
        // Validation checks
        if (levelIndex < 0 || levelIndex > levelData.levelPrefabs.Length)
            return;

        var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        for (int i = 0; i < levelData.levelPrefabs.Length; i++)
        {
            var level = levelsLoaded_[i];

            // Check for out of the range we want loaded
            if (i < levelIndex - 1 || i > levelIndex + 1)
            {
                if (level != null)
                    Destroy(level);

                continue;
            }
            // Else covers all levels we want loaded
            else if (level == null)
            {
                GameObject newLevel = Instantiate<GameObject>(levelData.levelPrefabs[i]);
                newLevel.gameObject.transform.position = new Vector3(0, i % 3 * 100, 0);
                if (i >= levelIndex && i - 1 >= 0)
                {
                    var previouslevel = levelsLoaded_[i - 1].GetComponentsInChildren<Level2D>()[0];
                    //TODO:checking for avatar not 1 is a hacky fix that will work for now
                    previouslevel.outBrokenComputer = newLevel
                        .GetComponentsInChildren<Computer>()
                        .ToList()
                        .Find((a) => a.isGhostComputer && a.avatar != 1);
                }

                UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(
                    newLevel,
                    currentScene
                );

                levelsLoaded_[i] = newLevel;
            }
        }
        cm_.BuildComputerLookUp();
    }

    public GameObject GetLevel(int levelIndex)
    {
        return levelsLoaded_[levelIndex];
    }
}
