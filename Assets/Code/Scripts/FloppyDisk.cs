using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Assertions;

public class FloppyDisk : MonoBehaviour, IUsable
{
    public Globals.FloppyDiskID floppyDiskID;
    Player player;
    private bool isInInventory_ = false;

    void Start()
    {
        player = FindFirstObjectByType<Player>();
        Assert.IsNotNull(player, "Unable to find Player from FloppyDisk.");
    }

    public string GetUsableLabel() => "Floppy Disk";

    public string GetActionLabel() => "Get";

    public void Use(IControllable p)
    {
        player.inventory.AddToInventory(this);
        isInInventory_ = true;
        // TODO: Clean this disgusting thing up
        FindFirstObjectByType<PlayerReacher>()
            ?.UnsetUsable(this);
    }

    public bool IsCurrentlyUsable()
    {
        return !isInInventory_;
    }

    public void SetVisible(bool visible = true)
    {
        this.gameObject.SetActive(visible);
    }

    public void UnsetVisible() => this.SetVisible(false);
}
