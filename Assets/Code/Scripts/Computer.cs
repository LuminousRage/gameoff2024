using System.Collections.Generic;
using System.Linq;
using Globals;
using UnityEngine;
using UnityEngine.Assertions;

public class Computer : MonoBehaviour, IUsable
{
    public Level2D level;
    public Globals.Zone zone;

    [Range(1, 8)]
    public byte avatar;

    private Camera renderCamera_;

    private GameObject watcher_;

    private GameObject quad_;

    private SceneManager sceneManager_;

    private ITriggerable triggerable;

    private List<FloppyDisk> floppyDisks = new List<FloppyDisk>();

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
        Assert.IsNotNull(this.level);
        NUnit.Framework.Assert.IsInstanceOf<Level2D>(this.level);

        renderCamera_ = GetComponentInChildren<Camera>();
        Assert.IsNotNull(renderCamera_, "Unable to find render camera in Computer.");

        watcher_ = this.transform.Find("Watcher")?.gameObject;
        Assert.IsNotNull(watcher_, "Unable to find watcher in Computer.");

        sceneManager_ = FindFirstObjectByType<SceneManager>();
        Assert.IsNotNull(sceneManager_, "Unable to find SceneManager from Computer.");

        quad_ = this.transform.Find("Blackscreen")?.gameObject;
        Assert.IsNotNull(quad_, "Unable to find Quad in Computer.");

        (avatarObj, _) = level.GetAndValidateAvatarAndZone(this);

        var cameraOffset = 2 * level.transform.forward;

        renderCamera_.transform.position = level.transform.position - cameraOffset;
        renderCamera_.transform.rotation = level.transform.rotation;

        ToggleComputer(false);
    }

    public void ToggleComputer(bool enabled = true)
    {
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

    public bool IsAvatarDisksFull() => avatarObj.IsKeysFull();

    public bool ContainsDisk() => floppyDisks.Count > 0;

    public void InsertFloppyDisk(FloppyDisk disk)
    {
        if (IsAvatarDisksFull())
        {
            Debug.LogError("Computer has max floppy disks inserted.");
            return;
        }

        avatarObj.AddKey(disk);
        floppyDisks.Add(disk);
    }

    public List<FloppyDisk> RemoveAllFloppyDisk()
    {
        floppyDisks.ForEach(d => avatarObj.RemoveKey(d));
        var disks = floppyDisks.ToList();
        floppyDisks.Clear();

        return disks;
    }
}
