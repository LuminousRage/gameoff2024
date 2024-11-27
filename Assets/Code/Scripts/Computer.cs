using UnityEngine;
using UnityEngine.Assertions;

public class Computer : MonoBehaviour, IUsable
{
    public Level2D level;
    public Globals.Zone zone;

    [Range(1, 8)]
    public byte avatar;

    private GameObject watcher_;

    public GameObject quad_;

    private SceneManager sceneManager_;

    private ITriggerable triggerable;

    public ComputerFloppyDisk floppyDiskManager { get; private set; }

    public bool isGhostComputer = false;

    private enum UseState
    {
        Usable,
        Broken,
    }

    private UseState state_ = UseState.Usable;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Assert.IsNotNull(
            this.level,
            $"{this} of {transform.parent.gameObject} has no level assigned."
        );
        NUnit.Framework.Assert.IsInstanceOf<Level2D>(this.level);

        watcher_ = this.transform.Find("Watcher")?.gameObject;
        Assert.IsNotNull(watcher_, "Unable to find watcher in Computer.");

        sceneManager_ = FindFirstObjectByType<SceneManager>();
        Assert.IsNotNull(sceneManager_, "Unable to find SceneManager from Computer.");

        floppyDiskManager = GetComponent<ComputerFloppyDisk>();
        Assert.IsNotNull(floppyDiskManager, "Unable to find ComputerFloppyDisk in Computer.");

        var currentAvatar = level.GetAndValidateAvatar(this);
        ToggleComputer(currentAvatar.az.currentZone == zone);
    }

    public void ToggleComputer(bool enabled = true)
    {
        Debug.Log($"Toggling computer {this} to {enabled}");
        quad_.SetActive(enabled);

        if (isGhostComputer)
        {
            // ghost computers should break on entry and exit!
            state_ = UseState.Broken;
        }

        if (triggerable != null)
        {
            if (enabled)
            {
                triggerable.Trigger();
            }
            else
            {
                triggerable.Untrigger();
            }
        }
    }
}
