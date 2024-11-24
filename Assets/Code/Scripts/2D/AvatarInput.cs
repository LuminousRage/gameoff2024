using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class AvatarInput : MonoBehaviour
{
    private Avatar avatar;
    private Rigidbody2D rb;
    public InputAction move;
    public InputAction standupAction;

    [SerializeField]
    private float speed = 5;

    void Start()
    {
        avatar = GetComponent<Avatar>();
        Assert.IsNotNull(this.avatar);

        move.Enable();
        standupAction.Enable();

        rb = avatar.GetComponent<Rigidbody2D>();
        Assert.IsNotNull(this.rb);

        standupAction.performed += context =>
        {
            if (avatar.GetControllable())
            {
                Debug.Log("Standing up");
                avatar.GetLevel().StandUp();
            }
        };
    }

    void FixedUpdate()
    {
        if (avatar.GetControllable())
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
}
