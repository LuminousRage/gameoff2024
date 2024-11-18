using UnityEngine;
using UnityEngine.InputSystem;

public class Avatar : MonoBehaviour
{
    public Rigidbody2D rb;
    public InputAction move;

    [SerializeField]
    private float speed = 5;

    void Start()
    {
        move.Enable();
        rb = GetComponent<Rigidbody2D>();
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
}
