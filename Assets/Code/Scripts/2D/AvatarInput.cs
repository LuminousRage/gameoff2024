using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class AvatarInput : MonoBehaviour, IUsableSetter
{
    private Avatar avatar;
    public InputAction move;
    public InputAction standupAction;
    public InputAction useAction;

    private IUsable usable;

    [SerializeField]
    private float speed = 5;

    void Start()
    {
        avatar = GetComponent<Avatar>();
        Assert.IsNotNull(this.avatar);

        move.Enable();
        standupAction.Enable();
        useAction.Enable();

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
        var rb = avatar.GetRigidbody();

        var directions = move.ReadValue<Vector2>();
        directions = new Vector2(Mathf.Round(directions.x), Mathf.Round(directions.y));
        var newPosition = rb.position + directions * Time.deltaTime * speed;
        rb.MovePosition(newPosition);
    }

    public void SetUsable(IUsable usable)
    {
        Debug.Log($"Setting PlayerReacher usable to {usable}");

        this.usable = usable;
        // this.sm_.SetUsePrompt(usable);
        return;
    }

    public void UnsetUsable(IUsable usable)
    {
        if (this.usable == usable)
        {
            Debug.Log($"Unsetting PlayerReacher usable from {usable}");
            this.usable = null;
            // this.sm_.UnsetUsePrompt();
        }
    }
}
