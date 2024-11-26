using Globals;
using UnityEngine;
using UnityEngine.Assertions;

public class Computer : MonoBehaviour, IUsable
{
    public Level2D level;
    public Zone zone;

    [Range(1, 8)]
    public byte avatar;

    private Camera renderCamera_;

    private GameObject watcher_;

    private GameObject quad_;

    private SceneManager sceneManager_;

    private ITriggerable triggerable;

    public ComputerFloppyDisk floppyDiskManager { get; private set; }

    private Avatar avatarObj;

    public void Use(IControllable p)
    {
        sceneManager_.SetFocus(this);
    }

    public string GetUsableLabel()
    {
        return "Computer";
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

        renderCamera_ = GetComponentInChildren<Camera>();
        Assert.IsNotNull(renderCamera_, "Unable to find render camera in Computer.");

        watcher_ = this.transform.Find("Watcher")?.gameObject;
        Assert.IsNotNull(watcher_, "Unable to find watcher in Computer.");

        sceneManager_ = FindFirstObjectByType<SceneManager>();
        Assert.IsNotNull(sceneManager_, "Unable to find SceneManager from Computer.");

        quad_ = this.transform.Find("Blackscreen")?.gameObject;
        Assert.IsNotNull(quad_, "Unable to find Quad in Computer.");

        floppyDiskManager = GetComponent<ComputerFloppyDisk>();
        Assert.IsNotNull(floppyDiskManager, "Unable to find ComputerFloppyDisk in Computer.");

        if (level != null)
        {
            avatarObj = level.GetAndValidateAvatarAndZone(this).Item1;

            var cameraOffset = 2 * level.transform.forward;

            renderCamera_.transform.position = level.transform.position - cameraOffset;
            renderCamera_.transform.rotation = level.transform.rotation;
        }

        ToggleComputer(false);
    }

    public void FixedUpdate()
    {
        if (avatarObj != null)
        {
            renderCamera_.transform.position =
                avatarObj.transform.position - 2 * level.transform.forward;
        }
    }

    public void ToggleComputer(bool enabled = true)
    {
        Debug.Log($"Toggling computer {this} to {enabled}");
        renderCamera_.gameObject.SetActive(enabled);
        quad_.SetActive(enabled);

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
