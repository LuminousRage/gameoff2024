using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using IHoldable = FloppyDisk;

public class PlayerInventory : MonoBehaviour
{
    private List<IHoldable> items_ = new List<IHoldable>();

    private int currentIndex_ = -1;

    [Range(0.3f, 1.0f)]
    public float distanceFromHead = 0.35f;

    [Range(0f, 1.0f)]
    public float distanceDown = 0.17f;

    [Range(0f, 1.0f)]
    public float distanceRight = -0.17f;

    private Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Player>();
        Assert.IsNotNull(player, "Unable to find Player from PlayerInventory.");
    }

    // Update is called once per frame
    void Update()
    {
        var currentHeldItem = GetCurrentHoldable();

        if (currentHeldItem == null)
        {
            return;
        }

        // This shouldn't happen, updating item active just in case
        if (!currentHeldItem.gameObject.activeSelf)
        {
            UpdatePlayerHeldItem();
        }

        var headTransform_ = player.GetHeadTransform();
        if (headTransform_ == null)
        {
            Debug.LogError("Unable to get the head transform from the player.");
            return;
        }

        var newPosition =
            headTransform_.position
            + this.distanceFromHead * headTransform_.forward
            - distanceDown * headTransform_.up
            + distanceRight * headTransform_.right;
        currentHeldItem.transform.position = newPosition;
        currentHeldItem.transform.rotation = headTransform_.rotation * Quaternion.Euler(100, 0, 0);
    }

    public IHoldable GetCurrentHoldable()
    {
        if (currentIndex_ == -1)
            return null;

        if (currentIndex_ >= items_.Count)
        {
            Debug.LogError($"Current index {currentIndex_} is out of bounds, resetting");
            currentIndex_ = items_.Count - 1;
        }

        return this.items_[currentIndex_];
    }

    public void AddToInventory(IHoldable disk)
    {
        if (items_.Contains(disk))
        {
            Debug.LogError($"Disk {disk} already in inventory.");
            return;
        }

        // if (player.reacher_.usable_.Contains(disk))
        // {
        //     player.reacher_.usable_.Remove(disk);
        // }
        // ;
        Debug.Log($"Adding disk {disk} to inventory.");
        // I think we'd only use the ID, but we can change that later
        items_.Add(disk);

        currentIndex_ = items_.Count - 1;
        disk.SetVisible(true);
    }

    public IHoldable PopCurrentHoldableFromInventory()
    {
        var disk = GetCurrentHoldable();
        if (currentIndex_ == -1)
        {
            return null;
        }

        if (disk == null)
        {
            Debug.LogError($"Disk {disk} not in inventory.");
            return null;
        }

        Debug.Log($"Removing disk {disk} from inventory.");
        items_.Remove(disk);
        currentIndex_ -= 1;
        UpdatePlayerHeldItem();

        return disk;
    }

    public void UpdatePlayerHeldItem()
    {
        items_.ForEach(holdable => holdable.SetVisible(false));
        if (currentIndex_ == -1)
        {
            Debug.Log("Player is now holding nothing.");
            return;
        }

        var currentHeldItem = GetCurrentHoldable();
        if (currentHeldItem == null)
        {
            Debug.LogWarning("Unable to get the current held item.");
            return;
        }

        currentHeldItem.SetVisible(true);

        Debug.Log($"Player is now holding {currentHeldItem}");
    }

    public void PutDownDisks(Computer c)
    {
        if (c == null)
        {
            Debug.LogError("Computer is null.");
            return;
        }

        var disks = items_.ToArray();
        items_.Clear();
        currentIndex_ = -1;

        foreach (var disk in disks)
        {
            disk.isInInventory_ = false;
            var pos = c.transform.position + new Vector3(0f, -0.145f, 1f);
            disk.transform.SetPositionAndRotation(pos, Quaternion.Euler(0, 0, 0));
        }
    }
    // Below is unused since we no longer hold multiple disks
    // public void SelectItem(int index)
    // {
    //     if (index < 0 || index >= items_.Count)
    //     {
    //         Debug.LogError($"Invalid index {index} for selecting an item.");
    //         return;
    //     }

    //     currentIndex_ = index;
    //     UpdatePlayerHeldItem();
    // }

    // public void SelectNext()
    // {
    //     var newIndex = currentIndex_ + 1;

    //     if (items_.Count == 0 || currentIndex_ >= items_.Count - 1)
    //     {
    //         newIndex = -1;
    //     }

    //     this.SelectItem(newIndex);
    // }

    // public void SelectPrevious()
    // {
    //     var newIndex = currentIndex_ - 1;

    //     if (items_.Count == 0 || currentIndex_ == 0)
    //     {
    //         newIndex = -1;
    //     }

    //     this.SelectItem(newIndex);
    // }

    // public void Unselect()
    // {
    //     this.SelectItem(-1);
    // }
}
