using UnityEngine;
using UnityEngine.Assertions;

public class FloppyDisk : MonoBehaviour, IUsable
{
    public Globals.FloppyDiskID floppyDiskID;
    Player player;

    void Start()
    {
        player = FindFirstObjectByType<Player>();
        Assert.IsNotNull(player, "Unable to find Player from FloppyDisk.");
    }

    public string GetUsableLabel() => "Floppy Disk";

    public string GetActionLabel() => "Get";

    public void Use(IControllable p)
    {
        player.AddToInventory(this);
        // Remove the floppy disk from the scene
    }
}
