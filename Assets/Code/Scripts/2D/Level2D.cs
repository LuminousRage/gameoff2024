using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Level2D : MonoBehaviour
{
    bool entered_ = false;
    Vector2 transformPosition_;

    private SceneManager sceneManager;
    private ComputerManager computerManager;
    private GhostFloppyDiskManager ghostFloppyDiskManager;

    // For broken zones only - the computer all avatars should exit out at
    public Computer outBrokenComputer;

    [Range(0, 15)]
    public int levelOrder;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transformPosition_ = new Vector2(transform.position.x, transform.position.y);
        sceneManager = GameObject.FindFirstObjectByType<SceneManager>();
        Assert.IsNotNull(this.sceneManager);
        computerManager = GameObject.FindFirstObjectByType<ComputerManager>();
        Assert.IsNotNull(this.computerManager);
        ghostFloppyDiskManager = FindFirstObjectByType<GhostFloppyDiskManager>();
        Assert.IsNotNull(this.ghostFloppyDiskManager);

        Assert.IsNotNull(outBrokenComputer, "No outBrokenComputer set for Level2D.");
    }

    // Update is called once per frame
    void Update() { }

    public void EnterFrom(Computer c)
    {
        if (c == null)
        {
            Debug.LogError("Attempted to enter level from a null Computer. Continuing anyway.");
            return;
        }
        var avatar = GetAndValidateAvatar(c);
        if (avatar == null)
        {
            Debug.LogError("Failed to enter level. Exiting.");
            return;
        }

        var avatarLastZone = avatar.az.GetZone();

        // Avatar was previously exited out in a different zone
        if (avatarLastZone != c.zone)
        {
            avatar.az.respawnIn(c.zone);
        }

        avatar.GetLevel().UpdatePlayerToComputer(avatar, c.zone, avatarLastZone);
        avatar.SetControllable(true);

        if (!entered_)
        {
            var avatars = GetComponentsInChildren<Avatar>().ToList();
            avatars.ForEach(a => a.SetRenderCamera(true));
            PlayerPrefs.SetInt("currentLevel", levelOrder);
        }
        entered_ = true;
    }

    public Avatar GetAndValidateAvatar(Computer c)
    {
        var avatars = GetComponentsInChildren<Avatar>().Where(a => a.number == c.avatar).ToList();

        if (avatars.Count != 1)
        {
            Debug.LogError(
                "Expected 1 avatar per computer for entering a level."
                    + $"Found {avatars.Count} avatars "
            );

            return null;
        }

        return avatars[0];
    }

    public void Exit()
    {
        if (!entered_)
        {
            Debug.LogError("Attempted to exit level without entering. Exiting anyway.");
        }

        var avatars = GetComponentsInChildren<Avatar>().ToList();
        avatars.ForEach(a =>
        {
            a.SetControllable(false);
            a.SetRenderCamera(false);
        });
        entered_ = false;
    }

    public void StandUp()
    {
        sceneManager.SetFocus(null);
    }

    public void UpdatePlayerToComputer(
        Avatar avatar,
        Globals.Zone newZone,
        Globals.Zone? oldZone,
        bool computerOverride = false
    )
    {
        if (avatar.hasEntered && oldZone != null)
        {
            var originalComputer = computerManager.computerLookUp[this][
                (avatar.number, oldZone.Value)
            ];
            // If this is implemented right, every computer of the same avatar ID should be turned off when exited
            originalComputer.ToggleComputer(false);
        }
        else
        {
            avatar.hasEntered = true;
        }

        // Current design doesn't allow for calculating computerOverride condition, so it has to be passed in
        Computer computer = computerOverride
            ? outBrokenComputer
            : computerManager.computerLookUp[this][(avatar.number, newZone)];
        computer.ToggleComputer(true);

        // move 3d player in front of computer
        Debug.Log($"Moving player to {computer} of {computer.transform.parent.name}");
        var transform = computer.GetWatcherTransform();
        sceneManager.UpdatePlayerLocation(transform);
    }

    public void TellOtherComputersToRenderGhostDisks(
        byte avatarId,
        Computer originComputer,
        int slotIndex,
        bool setVisible = true
    )
    {
        var computers = computerManager
            .computerLookUp[this]
            .Where(c => c.Key.Item1 == avatarId && c.Value != originComputer)
            .Select(c => c.Value);

        foreach (var computer in computers)
        {
            if (setVisible)
            {
                var ghostDisk = ghostFloppyDiskManager.GetUnusedGhostDisk();
                computer.floppyDiskManager.SetGhostFloppyDisk(ghostDisk, slotIndex);
                Debug.Log(computer.floppyDiskManager.GetSlotPosition(slotIndex));
                ghostDisk.transform.SetPositionAndRotation(
                    computer.floppyDiskManager.GetSlotPosition(slotIndex),
                    Quaternion.Euler(0, 0, 0)
                );
            }
            else
            {
                GameObject ghostFloppy = computer.floppyDiskManager.GetGhostFloppyDisk(slotIndex);
                ghostFloppyDiskManager.ReturnGhostDisk(ghostFloppy);
            }
        }
    }
}
