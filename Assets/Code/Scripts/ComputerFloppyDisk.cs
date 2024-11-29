using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class ComputerFloppyDisk : MonoBehaviour
{
    public Vector3[] slotPosition = new Vector3[2]
    {
        new Vector3(0.1352f, 0.087f, 0.1534f),
        new Vector3(-0.0011f, 0.0338f, 0.1534f),
    };

    private Avatar avatar;
    private Computer computer;

    // this is a reference, so will be updated when the avatar's keys are updated
    private FloppyDisk[] avatarFloppyDisks;
    private GameObject[] ghostFloppyDisks = new GameObject[2];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        computer = this.GetComponentInParent<Computer>();
        Assert.IsNotNull(this.computer);

        if (computer.level == null)
        {
            return;
        }
        avatar = computer.level.GetAndValidateAvatar(this.computer);
        Assert.IsNotNull(this.avatar, "Unable to find avatar from ComputerFloppyDisk.");
        avatarFloppyDisks = avatar.GetKeys();
    }

    // Update is called once per frame
    void Update() { }

    public bool IsAvatarDisksFull() => avatar.IsKeysFull();

    public FloppyDisk[] GetAllComputerDisks() =>
        avatarFloppyDisks.Where(d => d != null && d.GetComputer() == this.computer).ToArray();

    public bool ContainsDisk() => GetAllComputerDisks().Count() > 0;

    public void InsertFloppyDisk(FloppyDisk disk)
    {
        if (IsAvatarDisksFull())
        {
            Debug.LogError("Computer has max floppy disks inserted.");
            return;
        }

        disk.SetComputer(this.computer);
        avatar.AddKey(disk);
        disk.doors.ToList().ForEach(door => door.UpdateLaserCollision(avatar));
    }

    public List<FloppyDisk> RemoveAllFloppyDisk()
    {
        var allDisks = GetAllComputerDisks().ToList();
        allDisks.ForEach(d =>
        {
            avatar.RemoveKey(d);
            d.SetComputer(null);
            d.doors.ToList().ForEach(door => door.UpdateLaserCollision(avatar));
        });

        return allDisks;
    }

    public Vector3 GetSlotPosition(int slotIndex) => slotPosition[slotIndex] + transform.position;

    public void SetGhostFloppyDisk(GameObject disk, int index)
    {
        ghostFloppyDisks[index] = disk;
    }

    public GameObject GetGhostFloppyDisk(int index) => ghostFloppyDisks[index];
}
