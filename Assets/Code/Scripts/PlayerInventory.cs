using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using IHoldable = FloppyDisk;

public class PlayerInventory : MonoBehaviour
{
    private List<IHoldable> items_ = new List<IHoldable>();

    private int currentIndex_ = -1;

    [Range(0f, 1.0f)]
    public float distanceFromHead = 0.3f;

    private Transform headTransform_;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var player = FindFirstObjectByType<Player>();

        this.headTransform_ = player.GetHeadTransform();
        Assert.IsNotNull(headTransform_, "Unable to find head transform from the player.");
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

        var newPosition = headTransform_.position + this.distanceFromHead * headTransform_.forward;

        currentHeldItem.transform.position = newPosition;
        currentHeldItem.transform.rotation = headTransform_.rotation;
    }

    private IHoldable GetCurrentHoldable()
    {
        if (currentIndex_ == -1)
            return null;

        return this.items_[currentIndex_];
    }

    public void AddToInventory(IHoldable disk)
    {
        if (items_.Contains(disk))
        {
            Debug.LogError($"Disk {disk} already in inventory.");
            return;
        }
        Debug.Log($"Adding disk {disk} to inventory.");
        // I think we'd only use the ID, but we can change that later
        items_.Add(disk);

        disk.SetVisible(false);
    }

    public List<IHoldable> GetInventory()
    {
        return items_;
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

    public void SelectItem(int index)
    {
        if (index < 0 || index >= items_.Count)
        {
            Debug.LogError($"Invalid index {index} for selecting an item.");
            return;
        }

        currentIndex_ = index;
        UpdatePlayerHeldItem();
    }

    public void SelectNext()
    {
        var newIndex = currentIndex_ + 1;

        if (items_.Count == 0 || currentIndex_ >= items_.Count - 1)
        {
            newIndex = -1;
        }

        this.SelectItem(newIndex);
    }

    public void SelectPrevious()
    {
        var newIndex = currentIndex_ - 1;

        if (items_.Count == 0 || currentIndex_ == 0)
        {
            newIndex = -1;
        }

        this.SelectItem(newIndex);
    }

    public void Unselect()
    {
        this.SelectItem(-1);
    }
}
