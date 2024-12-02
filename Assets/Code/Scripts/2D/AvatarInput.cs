using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class AvatarInput : MonoBehaviour, IUsableSetter
{
    private Avatar avatar;
    public InputAction move;
    public InputAction standupAction;
    public InputAction useAction;

    public IUsable usable { get; private set; }

    [SerializeField]
    private float speed = 5;

    public Animator animator;
    SceneManager sm;

    void Start()
    {
        animator = GetComponent<Animator>();
        Assert.IsNotNull(animator);
        avatar = GetComponent<Avatar>();
        Assert.IsNotNull(this.avatar);

        move.Enable();
        standupAction.Enable();
        useAction.Enable();

        sm = FindFirstObjectByType<SceneManager>();
        Assert.IsNotNull(sm);

        standupAction.performed += context =>
        {
            if (avatar.GetControllable() && avatar.az.currentCollisionZone.isStandUpable)
                sm.RunActionWithInputLock(
                    () =>
                    {
                        Debug.Log("Standing up");
                        avatar.GetLevel().StandUp();
                    },
                    context.action
                );
        };

        useAction.performed += context =>
        {
            if (avatar.GetControllable())
            {
                usable?.Use(null);
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

        var direction = move.ReadValue<Vector2>();
        // Debug.Log(animator.GetBool("IsWalking"));

        if (direction.x != 0 || direction.y != 0)
        {
            animator.SetFloat("X", direction.x);
            animator.SetFloat("Y", direction.y);

            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        direction = new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));
        var newPosition = rb.position + direction * Time.deltaTime * speed;
        rb.MovePosition(newPosition);
    }

    public void SetUsable(IUsable usable)
    {
        Debug.Log($"Setting AvatarInput usable to {usable}");

        this.usable = usable;
        // this.sm_.SetUsePrompt(usable);
        return;
    }

    public void UnsetUsable(IUsable usable)
    {
        if (this.usable == usable)
        {
            Debug.Log($"Unsetting AvatarInput usable from {usable}");
            this.usable = null;
            // this.sm_.UnsetUsePrompt();
        }
    }

    public void SetInputEnable(bool enable)
    {
        if (enable)
        {
            useAction.Enable();
            move.Enable();
            standupAction.Enable();
        }
        else
        {
            useAction.Disable();
            move.Disable();
            standupAction.Disable();
        }
    }
}
