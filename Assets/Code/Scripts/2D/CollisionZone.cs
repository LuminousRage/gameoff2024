using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class CollisionZone : MonoBehaviour
{
    public Globals.Zone zone;

    private Collider2D c;

    public GameObject avatarSpawnPoint;


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
