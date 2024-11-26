using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PlayerReacher : MonoBehaviour, IUsableSetter
{
    private Player player_;
    private List<IUsable> usable_ = new List<IUsable>();

    private SceneManager sm_;

    private Computer computer_;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player_ = this.transform.GetComponentInParent<Player>();
        Assert.IsNotNull(player_, "Unable to find Player script from PlayerReacher.");

        sm_ = GameObject.FindFirstObjectByType<SceneManager>();
        Assert.IsNotNull(sm_, "Unable to find SceneManager from Player.");
    }

    // Update is called once per frame
    void Update() { }

    public void UseUsable()
    {
        if (this.usable_.Count == 0)
        {
            return;
        }

        Debug.Log($"Using usable: {this.usable_}");
        this.usable_[0]?.Use(this.player_);
    }

    public void SetUsable(IUsable usable)
    {
        if (this.usable_.Contains(usable))
        {
            return;
        }
        Debug.Log($"Setting PlayerReacher usable to {usable}");
        this.usable_.Add(usable);
        return;
    }

    public void UnsetUsable(IUsable usable)
    {
        if (this.usable_.Contains(usable))
        {
            Debug.Log($"Unsetting PlayerReacher usable from {usable}");
            this.usable_.Remove(usable);
        }
    }

    public List<IUsable> SortUsablePriority()
    {
        return this.usable_;
    }

    void OnTriggerEnter(Collider c)
    {
        var usable = c.GetComponent<IUsable>();
        if (usable == null)
        {
            // Debug.Log("Collision occured with non IUsable.");
            return;
        }
        if (usable.IsCurrentlyUsable())
        {
            this.SetUsable(usable);
        }

        // computer is a usable, so no fear of early return
        var comp = c.GetComponent<Computer>();
        if (comp != null)
        {
            computer_ = comp;

            if (GetDiskActionPrompt() == DiskActionPrompts.None)
            {
                computer_ = null;
                return;
            }
        }
    }

    void OnTriggerExit(Collider c)
    {
        var usable = c.GetComponent<IUsable>();
        if (usable == null)
        {
            Debug.Log("Collision occured with non IUsable.");
            return;
        }

        this.UnsetUsable(usable);

        var comp = c.GetComponent<Computer>();
        if (comp != null)
        {
            computer_ = null;
        }
    }

    public enum DiskActionPrompts
    {
        Insert,
        Eject,

        // not usable, but will show a prompt
        CannotInsert,
        None,
    }

    public (string, string) DiskActionPromptToString(DiskActionPrompts prompt)
    {
        switch (prompt)
        {
            case DiskActionPrompts.Insert:
                return ("F", "Insert floppy disk");
            case DiskActionPrompts.Eject:
                return ("F", "Eject floppy disk");
            case DiskActionPrompts.CannotInsert:
                return (null, "Floppy disk drive is full");
            case DiskActionPrompts.None:
                return (null, null);
            default:
                return (null, null);
        }
    }

    public DiskActionPrompts GetDiskActionPrompt()
    {
        if (computer_ == null)
        {
            return DiskActionPrompts.None;
        }

        switch
            (
                computer_.IsAvatarDisksFull(),
                computer_.ContainsDisk(),
                player_.inventory.GetCurrentHoldable() != null
            )

        {
            case (true, false, true):
                return DiskActionPrompts.CannotInsert;
            // false, true, true
            // false, false, true
            case (false, _, true):
                return DiskActionPrompts.Insert;
            case (true, true, true):
                // We can't handle this case because we can only hold one disk at a time.
                // But also Louis said this won't ever happen in the game
                // To avoid being in an unsolvable state, let's not do anything to the disk
                Debug.LogError(
                    "Computer has an ejectable disk even though we are currently holding a disk."
                );
                return DiskActionPrompts.None;
            // false, true, false
            // true, true, false
            case (_, true, _):
                return DiskActionPrompts.Eject;
            // false, false, false
            // true, false, false
            default:
                return DiskActionPrompts.None;
        }
    }

    public void UseDiskAction()
    {
        if (computer_ == null)
        {
            return;
        }

        var prompt = GetDiskActionPrompt();
        Debug.Log($"Using disk action {prompt}");

        switch (prompt)
        {
            case DiskActionPrompts.Insert:
                var disk = player_.inventory.PopCurrentHoldableFromInventory();
                computer_.InsertFloppyDisk(disk);
                break;
            case DiskActionPrompts.Eject:
                var disks = computer_.RemoveAllFloppyDisk();
                disks.ForEach(disk => player_.inventory.AddToInventory(disk));
                break;
            case DiskActionPrompts.CannotInsert:
                break;
            case DiskActionPrompts.None:
                break;
            default:
                Debug.LogError("Unknown disk action prompt.");
                break;
        }
    }
}
