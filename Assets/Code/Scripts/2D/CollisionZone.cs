using UnityEngine;
using UnityEngine.Assertions;

public class CollisionZone : MonoBehaviour
{
    public Globals.Zone zone;

    private Collider2D c;

    public SpawnPoint avatarSpawnPoint;

    public bool isStandUpable = true;

    void Start()
    {
        c = GetComponent<CompositeCollider2D>();
        Assert.IsNotNull(c);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var avatar = other.GetComponent<Avatar>();
            var centre = other.offset + other.attachedRigidbody.position;
            if (avatar.az.GetZone() != zone && c.OverlapPoint(centre))
            {
                avatar.az.changeAreaZone(this);
            }
        }
    }
}
