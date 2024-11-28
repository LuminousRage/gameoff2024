using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AvatarZone : MonoBehaviour
{
    private Avatar avatar;

    // This should only be used in Start functions - everything else refer to currentCollisionZone.zone
    public Globals.Zone currentZone;
    public Dictionary<Globals.Zone, GameObject> spawns;

    [System.Serializable]
    public struct SpawnPoint
    {
        public Globals.Zone zone;
        public GameObject spawn;
    }

    public SpawnPoint[] intialSpawns;

    public CollisionZone currentCollisionZone;

    void Start()
    {
        avatar = GetComponent<Avatar>();
        Assert.IsNotNull(this.avatar);

        spawns = new Dictionary<Globals.Zone, GameObject>();
        foreach (var sp in intialSpawns)
        {
            spawns[sp.zone] = sp.spawn;
        }

        MoveAvatarTo(spawns[currentZone].transform.position);
    }

    public void MoveAvatarTo(Vector2 position)
    {
        avatar.transform.position = position;
    }

    public void changeAreaZone(CollisionZone newArea)
    {
        var oldZone = currentCollisionZone?.zone;
        if (avatar.GetControllable())
        {
            Debug.Log($"Changing Avatar {avatar.number} zone from {oldZone} to {newArea.zone}");
            avatar.GetLevel().UpdatePlayerToComputer(avatar, newArea.zone, oldZone);
        }

        // maybe todo: remove currentZone and get it from currentCollisionZone
        currentCollisionZone = newArea;
        spawns[newArea.zone] = newArea.avatarSpawnPoint;
    }

    public void respawnIn(Globals.Zone zone)
    {
        var spawnPoint = new Vector2(
            spawns[zone].transform.position.x,
            spawns[zone].transform.position.y
        );
        Debug.Log($"Respawning avatar {avatar.number} to ${spawnPoint}");

        MoveAvatarTo(spawnPoint);
    }

    public Globals.Zone? GetZone()
    {
        if (currentCollisionZone == null)
        {
            return null;
        }
        return currentCollisionZone.zone;
    }
}
