using UnityEngine;
using UnityEngine.Assertions;

public class Computer : MonoBehaviour, IUsable
{
    public Level2D level;
    public Globals.Zone zone;

    [Range(1, 8)]
    public byte avatar;

    public GameObject watcher_;

    public GameObject quad_;
    public Material[] avatarScreens;

    private SceneManager sceneManager_;

    public Triggerable triggerable;
    public ComputerSoundManager soundManager;

    public ComputerFloppyDisk floppyDiskManager { get; private set; }

    public Avatar avatarObj { get; private set; }

    public bool isGhostComputer = false;

    public enum UseState
    {
        Usable,
        Broken,
    }

    [HideInInspector]
    public UseState state_ = UseState.Usable;

    public void Use(IControllable p)
    {
        switch (state_)
        {
            case UseState.Usable:
                sceneManager_.SetFocus(this);
                break;
            case UseState.Broken:
                break;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Assert.IsNotNull(
            this.level,
            $"{this} of {transform.parent.gameObject} has no level assigned."
        );
        // NUnit.Framework.Assert.IsInstanceOf<Level2D>(this.level);

        Assert.IsNotNull(watcher_, "Unable to find watcher in Computer.");

        sceneManager_ = FindFirstObjectByType<SceneManager>();
        Assert.IsNotNull(sceneManager_, "Unable to find SceneManager from Computer.");

        floppyDiskManager = GetComponentInChildren<ComputerFloppyDisk>();
        Assert.IsNotNull(floppyDiskManager, "Unable to find ComputerFloppyDisk in Computer.");

        var currentAvatar = level.GetAndValidateAvatar(this);
        ToggleComputer(currentAvatar.az.currentCollisionZone.zone == zone, true);

        quad_.GetComponent<MeshRenderer>().material = avatarScreens[avatar - 1];

        avatarObj = level.GetAndValidateAvatar(this);
        Assert.IsNotNull(this.avatarObj, "Unable to find avatar from Computer.");

        Assert.IsNotNull(this.soundManager, "Unable to find sound manager");
    }

    public void ToggleComputer(bool enabled = true, bool firstToggle = false)
    {
        if (!firstToggle)
        {
            Debug.Log($"Toggling computer {this} to {enabled}");
        }

        quad_.SetActive(enabled);

        if (enabled)
        {
            soundManager.ToggleOn();
        }
        
        if (firstToggle)
            return;
        
        if (!enabled)
        {
            soundManager.ToggleOff();
        }

        if (isGhostComputer)
        {
            Debug.Log($"Toggling {this} to broken");
            // ghost computers should break on entry and exit!
            state_ = UseState.Broken;
        }

        if (triggerable != null)
        {
            // Computer off trigers the triggerable
            if (!enabled)
            {
                triggerable.Trigger();
            }
            else
            {
                triggerable.Untrigger();
            }
        }
    }

    public string GetUsableLabel()
    {
        switch (state_)
        {
            case UseState.Usable:
                return "Computer";
            case UseState.Broken:
                return "The computer is broken...";
            default:
                Debug.LogError($"Invalid state {state_} for {this}");
                return "Computer";
        }
    }

    public string GetActionLabel()
    {
        switch (state_)
        {
            case UseState.Usable:
                return "Use";
            case UseState.Broken:
                return "";
            default:
                Debug.LogError($"Invalid state {state_} for {this}");
                return "Use";
        }
    }

    public string GetKeyLabel()
    {
        switch (state_)
        {
            case UseState.Usable:
                return "E";
            case UseState.Broken:
                return "";
            default:
                Debug.LogError($"Invalid state {state_} for {this}");
                return "E";
        }
    }

    public Transform GetWatcherTransform()
    {
        return watcher_.transform;
    }

    public override string ToString()
    {
        return $"Computer {avatar} (zone {zone}, {level.name})";
    }
}
