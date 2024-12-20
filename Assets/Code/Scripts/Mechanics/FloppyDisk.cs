using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class FloppyDisk : MonoBehaviour, IUsable
{
    public Globals.FloppyDiskID floppyDiskID;
    Player player;
    public bool isInInventory_ = false;
    public bool ghost = false;

    // The computer the floppy disk is currently in, null if not in a computer
    public Computer computer_;

    public LaserDoor[] doors = new LaserDoor[0];

    void Start()
    {
        var rend = GetComponentInChildren<MeshRenderer>();
        // Debug.Log($"{rend}");

        MaterialPropertyBlock mpb = new MaterialPropertyBlock();

        mpb.SetColor("_NewColor", Globals.GetFloppyColor(floppyDiskID));
        rend.SetPropertyBlock(mpb);
        //////
        doors.ToList().ForEach(door => door.color = Globals.GetFloppyColor(floppyDiskID));

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

    public bool IsCurrentlyUsable() => !isInInventory_ && computer_ == null && !ghost;

    public void SetVisible(bool visible = true) => this.gameObject.SetActive(visible);

    public void UnsetVisible() => this.SetVisible(false);

    public Computer GetComputer() => computer_;

    public void SetComputer(Computer computer) => computer_ = computer;

    public void SetFloppyDiskTransform(int slotIndex)
    {
        var (pos, rot) = computer_.floppyDiskManager.GetSlotPositionAndRotation(slotIndex);
        transform.SetPositionAndRotation(pos, rot);
    }
}
