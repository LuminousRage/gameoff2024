using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AvatarZone : MonoBehaviour
{
    private Avatar avatar;

    // This should only be used in Start functions - everything else refer to currentCollisionZone.zone
    public Dictionary<Globals.Zone, SpawnPoint> spawns;

    [System.Serializable]
    public struct SetSpawnPoint
    {
        public Globals.Zone zone;
        public SpawnPoint spawn;
    }

    public SetSpawnPoint[] intialSpawns;

    public CollisionZone currentCollisionZone;

    void Start()
    {
        avatar = GetComponent<Avatar>();
        Assert.IsNotNull(this.avatar);
        Assert.IsNotNull(this.currentCollisionZone);

        spawns = new Dictionary<Globals.Zone, SpawnPoint>();
        foreach (var sp in intialSpawns)
        {
            spawns[sp.zone] = sp.spawn;
            Assert.IsNotNull(sp.spawn, $"Spawn for avatar {avatar.number} zone {sp.zone} is null!");
            sp.spawn.changeSpawn(true, avatar.number);
        }
    }

    public void MoveAvatarTo(Vector2 position)
    {
        avatar.transform.position = new Vector3(position.x, position.y, avatar.transform.position.z);
    }

    public void changeAreaZone(CollisionZone newArea)
    {
        var oldZone = currentCollisionZone?.zone;
        if (avatar.GetControllable() && newArea.isStandUpable)
        {
            var computerOverride = newArea.zone == Globals.Zone.Broken;
            avatar
                .GetLevel()
                .UpdatePlayerToComputer(avatar, newArea.zone, oldZone, computerOverride);
        }

        Debug.Log($"Changing Avatar {avatar.number} zone from {oldZone} to {newArea.zone}");

        if (spawns[newArea.zone] != null)
        {
            spawns[newArea.zone].changeSpawn(false, avatar.number);
        }

        Assert.IsNotNull(
            newArea.avatarSpawnPoint,
            $"Avatar spawn point for zone {newArea.zone} is null!"
        );
        newArea.avatarSpawnPoint.changeSpawn(true, avatar.number);

        currentCollisionZone = newArea;
        spawns[newArea.zone] = newArea.avatarSpawnPoint;
    }

    public void respawnIn(Globals.Zone zone)
    {
        var spawnPoint = new Vector2(
            spawns[zone].transform.position.x + 0.5f,
            spawns[zone].transform.position.y + 0.75f
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
