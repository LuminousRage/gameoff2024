using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class ComputerFloppyDisk : MonoBehaviour
{
    public Vector3[] slotPosition = new Vector3[2]
    {
        new Vector3(-0.0481f, 0.1f, 0.08f),
        new Vector3(-0.1651f, 0.1f, 0.0802f),
    };
    public Quaternion defaultDiskRotation = Quaternion.Euler(-90, 90, 0);

    private Computer computer;

    // this is a reference, so will be updated when the avatar's keys are updated
    private FloppyDisk[] avatarFloppyDisks;
    private GameObject[] ghostFloppyDisks = new GameObject[2];
    private Avatar avatar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        computer = this.GetComponentInParent<Computer>();
        Assert.IsNotNull(this.computer);
        avatar = computer.level.GetAndValidateAvatar(computer);
        Assert.IsNotNull(this.avatar, $"{computer} has no avatar object.");
        avatarFloppyDisks = avatar.GetKeys();
    }

    public bool IsAvatarDisksFull() => avatar.IsKeysFull();

    public FloppyDisk[] GetAllComputerDisks() =>
        avatarFloppyDisks.Where(d => d != null && d.GetComputer() == this.computer).ToArray();

    public bool ContainsDisk() => GetAllComputerDisks().Count() > 0;

    public bool ContainsGhostDisk() => ghostFloppyDisks.Any(d => d != null);

    public void InsertFloppyDisk(FloppyDisk disk)
    {
        if (IsAvatarDisksFull())
        {
            Debug.LogError("Computer has max floppy disks inserted.");
            return;
        }

        disk.SetComputer(this.computer);
        avatar.AddKey(disk);
        disk.doors.ToList().ForEach(door => door.UpdateLaserCollision(avatar, true));
    }

    public List<FloppyDisk> RemoveAllFloppyDisk()
    {
        var allDisks = GetAllComputerDisks().ToList();
        allDisks.ForEach(d =>
        {
            avatar.RemoveKey(d);
            d.SetComputer(null);
            d.doors.ToList().ForEach(door => door.UpdateLaserCollision(avatar, false));
        });

        return allDisks;
    }

    public (Vector3 position, Quaternion rotation) GetSlotPositionAndRotation(int slotIndex)
    {
        var pos = this.transform.TransformPoint(slotPosition[slotIndex]);
        return (pos, transform.rotation * defaultDiskRotation);
    }

    public void SetGhostFloppyDisk(GameObject disk, int index)
    {
        ghostFloppyDisks[index] = disk;
    }

    public GameObject GetGhostFloppyDisk(int index) => ghostFloppyDisks[index];
}
