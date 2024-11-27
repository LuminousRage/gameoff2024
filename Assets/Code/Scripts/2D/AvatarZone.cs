using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

public class AvatarZone : MonoBehaviour
{
    private Avatar avatar;

    private CollisionZone collisionZone;
    public Dictionary<Globals.Zone,GameObject> spawns;

    void Start()
    {
        avatar = GetComponent<Avatar>();
        Assert.IsNotNull(this.avatar);
    }

    public void changeAreaZone(CollisionZone newArea)
    {
        if (avatar.GetControllable())
        {
            Debug.Log($"Changing Avatar zone from {collisionZone.zone} to {newArea.zone}");
            avatar.GetLevel().UpdatePlayerToComputer(avatar.number, newArea.zone, collisionZone.zone);
            collisionZone = newArea;
            spawns[newArea.zone] = newArea.avatarSpawnPoint;
        }
    }

    public Globals.Zone? GetZone()
    {
        return collisionZone.zone;
    }
}
