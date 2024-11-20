using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class Avatar : MonoBehaviour, IControllable
{
    private Rigidbody2D rb;
    public InputAction move;

    private int zone = 0;

    [SerializeField]
    private float speed = 5;

    [Range(1, 8)]
    public byte number = 1;

    private bool controlling_ = false;

    [System.Serializable]
    public struct ZoneSpawnPoint
    {
        public Zone zone;
        public Vector2 spawnPoint;
    }

    public ZoneSpawnPoint[] spawnPoints;

    public void SetControllable(bool controllable = true)
    {
        this.controlling_ = controllable;
    }

    void Start()
    {
        move.Enable();

        // for debug only
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (scene.name.Contains("Demo"))
        {
            SetControllable(true);
        }

        rb = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(this.rb);

        ValidateSpawnPoints();
    }

    void FixedUpdate()
    {
        if (controlling_)
        {
            MovementHandler();
        }
    }

    void MovementHandler()
    {
        var directions = move.ReadValue<Vector2>();
        directions = new Vector2(Mathf.Round(directions.x), Mathf.Round(directions.y));
        var newPosition = rb.position + directions * Time.deltaTime * speed;
        rb.MovePosition(newPosition);
    }

    void ValidateSpawnPoints()
    {
        Assert.IsTrue(spawnPoints.Length > 0, "No spawn points set for Avatar.");

        spawnPoints
            .ToList()
            .ForEach(sp =>
            {
                Assert.IsNotNull(sp.zone, "Zone not set for spawn point.");
                Assert.IsTrue(
                    sp.zone.GetComponent<CompositeCollider2D>().OverlapPoint(sp.spawnPoint),
                    $"Spawn point {sp.spawnPoint} is not within zone {sp.zone}."
                );
            });
    }

    public void SetZone(int newZone)
    {
        Debug.Log($"Changing Avatar zone from {zone} to {newZone}");
        zone = newZone;
    }

    public int GetZone()
    {
        return zone;
    }
}
