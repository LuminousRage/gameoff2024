using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class Avatar : MonoBehaviour
{
    private Rigidbody2D rb;
    public InputAction move;

    private int zone = 0;

    [SerializeField]
    private float speed = 5;

    void Start()
    {
        move.Enable();

        rb = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(this.rb);
    }

    void FixedUpdate()
    {
        MovementHandler();
    }

    void MovementHandler()
    {
        var directions = move.ReadValue<Vector2>();
        directions = new Vector2(Mathf.Round(directions.x), Mathf.Round(directions.y));
        var newPosition = rb.position + directions * Time.deltaTime * speed;
        rb.MovePosition(newPosition);
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
