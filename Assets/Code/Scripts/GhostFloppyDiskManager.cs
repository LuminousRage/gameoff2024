using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// this is like a pool for the ghost floppy disks, so we don't have to instantiate them every time
// but i do feel it's kind of redundant since the game is very simple, meh
public class GhostFloppyDiskManager : MonoBehaviour
{
    public int initialFloppyDiskCount = 10;
    public GameObject ghostFloppyDiskPrefab;
    private List<GameObject> ghostDisks = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < initialFloppyDiskCount; i++)
        {
            var ghostDisk = Instantiate(ghostFloppyDiskPrefab, transform);
            ghostDisk.SetActive(false);
            ghostDisks.Add(ghostDisk);
        }
    }

    public GameObject GetUnusedGhostDisk()
    {
        var disk = ghostDisks.FirstOrDefault();
        if (disk != null)
        {
            ghostDisks.Remove(disk);
            disk.SetActive(true);
            return disk;
        }

        var newDisk = Instantiate(ghostFloppyDiskPrefab, transform);
        newDisk.SetActive(true);

        return newDisk;
    }

    public void ReturnGhostDisk(GameObject disk)
    {
        disk.SetActive(false);
        ghostDisks.Add(disk);
    }
}
