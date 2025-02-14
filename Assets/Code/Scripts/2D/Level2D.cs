using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using sm = UnityEngine.SceneManagement;

public class Level2D : MonoBehaviour
{
    Vector2 transformPosition_;

    private SceneManager sceneManager;
    private ComputerManager computerManager;
    private GhostFloppyDiskManager ghostFloppyDiskManager;

    // For broken zones only - the computer all avatars should exit out at
    public Computer outBrokenComputer;

    private bool isOnBrokenComputer = false;

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

        //Assert.IsNotNull(outBrokenComputer, "No outBrokenComputer set for Level2D.");
    }

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
        avatar.SetRenderCamera(true);
        avatar.SetControllable(true);
        avatar.GetRigidbody().bodyType = RigidbodyType2D.Dynamic;
        sceneManager.avatarActive = avatar;
    }

    public Avatar GetAndValidateAvatar(Computer c)
    {
        var avatars = GetComponentsInChildren<Avatar>().Where(a => a.number == c.avatar).ToList();

        if (avatars.Count != 1)
        {
            Debug.LogError(
                "Expected 1 avatar per computer for entering a level."
                    + $"Found {avatars.Count} avatars for computer {c.avatar} in {c.zone}"
            );

            return null;
        }

        return avatars[0];
    }

    public void OnStandUp()
    {
        var avatars = GetComponentsInChildren<Avatar>().ToList();
        avatars.ForEach(a =>
        {
            a.SetControllable(false);
            a.GetRigidbody().bodyType = RigidbodyType2D.Static;
        });

        if (isOnBrokenComputer)
        {
            outBrokenComputer.quad_.SetActive(false);
            outBrokenComputer.soundManager.ToggleOff();
            var levels = FindObjectsByType<Level2D>(FindObjectsSortMode.None).ToList();
            var nextlevel = levels.Find((a) => a.levelOrder == levelOrder + 1);
            if (nextlevel == null)
            {
                sceneManager.UnlockMouse();
                sm.SceneManager.LoadScene("End Dialogue");
            }
            else
            {
                var nextAvatars = nextlevel.GetComponentsInChildren<Avatar>().ToList();
                avatars.ForEach(a => a.SetRenderCamera(false));
                nextAvatars.ForEach(a =>
                {
                    a.SetRenderCamera(true);
                });
                PlayerPrefs.SetInt("ContinueLevel", levelOrder + 1);
                PlayerPrefs.Save();

                sceneManager.EnsureLoaded(nextlevel.levelOrder);
            }
        }
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
        if (computerOverride)
        {
            computer.quad_.GetComponent<MeshRenderer>().material = computer.avatarScreens[
                avatar.number - 1
            ];
        }
        isOnBrokenComputer = computerOverride;
        computer.ToggleComputer(true);

        // move 3d player in front of computer
        Debug.Log($"Moving player to {computer} of {computer.transform.parent.name}");
        var transform = computer.GetWatcherTransform();
        sceneManager.UpdatePlayerLocation(transform);
        sceneManager.followCamera_._followee = transform;
    }

    public void TellOtherComputersToRenderGhostDisks(
        byte avatarId,
        Computer originComputer,
        int slotIndex,
        Globals.FloppyDiskID? floppyDiskID = null
    )
    {
        var computers = computerManager
            .computerLookUp[this]
            .Where(c => c.Key.Item1 == avatarId && c.Value != originComputer)
            .Select(c => c.Value);

        foreach (var computer in computers)
        {
            if (computer.zone == Globals.Zone.Broken)
            {
                // skip ghost disk for broken computers
                continue;
            }

            if (floppyDiskID != null)
            {
                var ghostDisk = ghostFloppyDiskManager.GetUnusedGhostDisk();

                var rend = ghostDisk.GetComponentInChildren<MeshRenderer>();
                MaterialPropertyBlock mpb = new MaterialPropertyBlock();
                mpb.SetFloat("_Transparency", 0.4f);
                mpb.SetColor("_NewColor", Globals.GetFloppyColor(floppyDiskID.Value));
                rend.SetPropertyBlock(mpb);

                computer.floppyDiskManager.SetGhostFloppyDisk(ghostDisk, slotIndex);
                var (pos, rot) = computer.floppyDiskManager.GetSlotPositionAndRotation(slotIndex);
                ghostDisk.transform.SetPositionAndRotation(pos, rot);
            }
            else
            {
                GameObject ghostFloppy = computer.floppyDiskManager.GetGhostFloppyDisk(slotIndex);
                ghostFloppyDiskManager.ReturnGhostDisk(ghostFloppy);
            }
        }
    }
}
