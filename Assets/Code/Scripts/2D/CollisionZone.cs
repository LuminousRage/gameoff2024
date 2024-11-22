using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class CollisionZone : MonoBehaviour
{
    public Globals.Zone zone;

    private Collider2D c;

    [System.Serializable]
    public struct AvatarSpawnPoint
    {
        public byte avatarId;
        public Vector2 spawnPoint;
    }

    public AvatarSpawnPoint[] spawnPoints;

    void Start()
    {
        c = GetComponent<CompositeCollider2D>();
        Assert.IsNotNull(c);
        ValidateSpawnPoints();
    }

    void ValidateSpawnPoints()
    {
        Assert.IsTrue(spawnPoints.Length > 0, "No spawn points set for Avatar.");

        var avatars = spawnPoints.Select(sp => sp.avatarId);
        Assert.IsTrue(
            avatars.Distinct().Count() == avatars.Count(),
            "Duplicate avatar IDs found in spawn points."
        );

        spawnPoints
            .ToList()
            .ForEach(sp =>
            {
                Assert.IsTrue(
                    c.OverlapPoint(sp.spawnPoint),
                    $"Spawn point {sp.spawnPoint} is not within zone"
                );
            });
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var avatar = other.GetComponent<Avatar>();
            var centre = other.offset + other.attachedRigidbody.position;
            if (avatar.GetZone() != zone && c.OverlapPoint(centre))
            {
                avatar.SetZone(zone);
            }
        }
    }
}
