using UnityEngine;
using UnityEngine.Assertions;

public class Zone : MonoBehaviour
{
    [Range(0, 9)]
    public int zone = 0;

    private Collider2D c;

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
            if (avatar.GetZone() != zone && c.bounds.Contains(centre))
            {
                avatar.SetZone(zone);
            }
        }
    }
}
