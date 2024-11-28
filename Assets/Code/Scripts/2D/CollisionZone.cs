using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class CollisionZone : MonoBehaviour
{
    public Globals.Zone zone;

    private Collider2D c;

    public GameObject avatarSpawnPoint1;
    public GameObject avatarSpawnPoint2;
    public GameObject avatarSpawnPoint3;

    public bool isStandUpable = true;
    private Tilemap tilemap1;
    private Tilemap tilemap2;
    private Tilemap tilemap3;
    public TilePlus tile;


    void Start()
    {
        c = GetComponent<CompositeCollider2D>();
        Assert.IsNotNull(c);
        tilemap1 = avatarSpawnPoint1.GetComponent<Tilemap>();
        tilemap1.RefreshAllTiles();
        tilemap2 = avatarSpawnPoint2.GetComponent<Tilemap>();
        tilemap2.RefreshAllTiles();
        tilemap3 = avatarSpawnPoint3.GetComponent<Tilemap>();
        tilemap3.RefreshAllTiles();
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

    void changeSpawn(bool alive,int avatar) {
        tile.alive = alive;
        switch (avatar){
            case 1:
                tilemap1.RefreshAllTiles();
                break;
            case 2:
                tilemap2.RefreshAllTiles();
                break;
            case 3:
                tilemap3.RefreshAllTiles();
                break;
        }
    }
}
