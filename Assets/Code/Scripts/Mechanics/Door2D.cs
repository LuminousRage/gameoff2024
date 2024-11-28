using UnityEngine;

public class Door2D : Door
{
    [Header("Door Movement Settings")]
    [Tooltip("Total distance the door will slide")]
    public float slideDistance = 2f;

    [Tooltip("Speed of door sliding")]
    public float slideSpeed = 2f;
    public Vector2 direction = Vector2.left;

    private Vector2 closedPosition;
    private Vector2 openPosition;
    private bool isMoving = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + (direction.normalized * slideDistance);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            // Move door
            transform.position = Vector2.MoveTowards(
                transform.position,
                isOpen ? openPosition : closedPosition,
                slideSpeed * Time.deltaTime
            );

            if (
                Vector2.Distance(transform.position, isOpen ? openPosition : closedPosition) < 0.01f
            )
            {
                isMoving = false;
            }
        }
    }

    public override void CloseSesame()
    {
        isMoving = true;
    }

    public override void OpenSesame()
    {
        isMoving = true;
    }
}
