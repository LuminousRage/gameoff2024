using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Assertions;

public class FloppyDisk : MonoBehaviour, IUsable
{
    public Globals.FloppyDiskID floppyDiskID;
    Player player;
    public bool isInInventory_ = false;

    // The computer the floppy disk is currently in, null if not in a computer
    public Computer computer_;

    public LaserDoor[] doors;

    void Start()
    {
        var rend = GetComponentInChildren<MeshRenderer>();
        Debug.Log($"{rend}");

        MaterialPropertyBlock mpb = new MaterialPropertyBlock();

        mpb.SetColor("_NewColor", Globals.GetFloppyColor(floppyDiskID));
        rend.SetPropertyBlock(mpb);
        //////

        player = FindFirstObjectByType<Player>();
        Assert.IsNotNull(player, "Unable to find Player from FloppyDisk.");

        // If level predefines the floppy disk in a computer, add it to the computer
        if (computer_ != null)
        {
            computer_.floppyDiskManager.InsertFloppyDisk(this);
        }
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

    public bool IsCurrentlyUsable() => !isInInventory_;

    public void SetVisible(bool visible = true) => this.gameObject.SetActive(visible);

    public void UnsetVisible() => this.SetVisible(false);

    public Computer GetComputer() => computer_;

    public void SetComputer(Computer computer) => computer_ = computer;

    public void SetFloppyDiskTransform(int slotIndex)
    {
        transform.SetPositionAndRotation(
            computer_.floppyDiskManager.GetSlotPosition(slotIndex),
            Quaternion.Euler(0, 0, 0)
        );
    }
}
