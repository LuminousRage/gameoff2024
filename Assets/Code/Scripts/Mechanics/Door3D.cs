using UnityEngine;

public class Door3D : Door
{
    [Header("Door Movement Settings")]
    [Tooltip("Total distance the door will slide")]
    public float slideDistance = 2f;

    [Tooltip("Speed of door sliding")]
    public float slideSpeed = 2f;

    [Header("Optional Audio")]
    [Tooltip("Audio source for door sliding sound")]
    public AudioSource doorSoundSource;

    [Tooltip("Audio clip for door opening")]
    public AudioClip openSound;

    [Tooltip("Audio clip for door closing")]
    public AudioClip closeSound;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isMoving = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + (Vector3.right.normalized * slideDistance);
    }

    // Update is called once per frame
    void Update()
    {
        // Handle door movement
        if (isMoving)
        {
            // Smoothly move the door towards its target position
            transform.position = Vector3.MoveTowards(
                transform.position,
                isOpen ? openPosition : closedPosition,
                slideSpeed * Time.deltaTime
            );

            // Check if door has reached its destination
            if (
                Vector3.Distance(transform.position, isOpen ? openPosition : closedPosition) < 0.01f
            )
            {
                isMoving = false;
            }
        }
    }

    public override void CloseSesame()
    {
        throw new System.NotImplementedException();
    }

    public override void OpenSesame()
    {
        throw new System.NotImplementedException();
    }
}
