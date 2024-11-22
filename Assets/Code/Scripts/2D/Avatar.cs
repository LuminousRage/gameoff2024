using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class Avatar : MonoBehaviour, IControllable
{
    private Level2D level;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public InputAction move;
    public InputAction standupAction;

    private Globals.Zone? zone;

    [SerializeField]
    private float speed = 5;

    [Range(1, 8)]
    public byte number = 1;

    private bool controlling_ = false;

    public void SetControllable(bool controllable = true)
    {
        this.controlling_ = controllable;
    }

    void Start()
    {
        move.Enable();
        standupAction.Enable();

        // for debug only
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (scene.name.Contains("Demo"))
        {
            SetControllable(true);
        }

        rb = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(this.rb);

        sr = GetComponent<SpriteRenderer>();
        Assert.IsNotNull(this.sr);

        level = GetComponentInParent<Level2D>();
        Assert.IsNotNull(this.level);

        sr.enabled = false;

        standupAction.performed += context =>
        {
            if (controlling_)
            {
                Debug.Log("Standing up");
                level.StandUp();
            }
        };
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

    public void MoveAvatarTo(Vector2 position)
    {
        transform.position = position;
    }

    public void ToggleSpriteRenderer(bool enable = true)
    {
        sr.enabled = enable;
    }

    public void SetZone(Globals.Zone newZone)
    {
        if (controlling_)
        {
            Debug.Log($"Changing Avatar zone from {zone} to {newZone}");
            zone = newZone;
            level.UpdatePlayerLocation((this.number, newZone));
        }
    }

    public Globals.Zone? GetZone()
    {
        return zone;
    }
}
