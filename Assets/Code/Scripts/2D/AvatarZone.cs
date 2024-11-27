using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

public class AvatarZone : MonoBehaviour
{
    private Avatar avatar;

    public Globals.Zone currentZone;
    public Dictionary<Globals.Zone,GameObject> spawns;

    [System.Serializable]
    public struct SpawnPoint {
        public Globals.Zone zone;
        public GameObject spawn;
    }

    public SpawnPoint[] intialSpawns;

    void Start()
    {
        avatar = GetComponent<Avatar>();
        Assert.IsNotNull(this.avatar);
        spawns = new Dictionary<Globals.Zone,GameObject>();
        foreach (var sp in intialSpawns) {
            spawns[sp.zone] = sp.spawn;
        }
    }

    public void changeAreaZone(CollisionZone newArea)
    {
        if (avatar.GetControllable())
        {
            Debug.Log($"Changing Avatar zone from {currentZone} to {newArea.zone}");
            avatar.GetLevel().UpdatePlayerToComputer(avatar.number, newArea.zone, currentZone);
            currentZone = newArea.zone;
            spawns[newArea.zone] = newArea.avatarSpawnPoint;
        }
    }

    public void respawnIn(Globals.Zone zone)
    {
        
        var spawnPoint = new Vector2(spawns[zone].transform.position.x,spawns[zone].transform.position.y);
        Debug.Log($"Respawning avatar to ${spawnPoint}");
        
        avatar.GetLevel().UpdatePlayerToComputer(avatar.number, zone, currentZone);
        avatar.MoveAvatarTo(spawnPoint);
        currentZone = zone;
        
    }

    public Globals.Zone GetZone()
    {
        return currentZone;
    }
}
