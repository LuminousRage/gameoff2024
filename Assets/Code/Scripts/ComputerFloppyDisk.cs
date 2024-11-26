using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class ComputerFloppyDisk : MonoBehaviour
{
    private Avatar avatar;
    private Computer computer;
    private List<FloppyDisk> floppyDisks = new List<FloppyDisk>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        computer = this.GetComponent<Computer>();
        Assert.IsNotNull(this.computer);

        (avatar, _) = computer.level.GetAndValidateAvatarAndZone(this.computer);
    }

    // Update is called once per frame
    void Update() { }

    public bool IsAvatarDisksFull() => avatar.IsKeysFull();

    public bool ContainsDisk() => floppyDisks.Count > 0;

    public void InsertFloppyDisk(FloppyDisk disk)
    {
        if (IsAvatarDisksFull())
        {
            Debug.LogError("Computer has max floppy disks inserted.");
            return;
        }

        avatar.AddKey(disk);
        floppyDisks.Add(disk);
    }

    public List<FloppyDisk> RemoveAllFloppyDisk()
    {
        floppyDisks.ForEach(d => avatar.RemoveKey(d));
        // todo: i'm pretty sure this creates a new list but should double check
        var disks = floppyDisks.ToList();
        floppyDisks.Clear();

        return disks;
    }
}
