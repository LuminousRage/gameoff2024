using UnityEngine;
using UnityEngine.Assertions;

public class Door3D : Door
{
    [Header("Door Movement Settings")]
    [Tooltip("Total distance the door will slide")]
    public float slideDistance = 1.95f;

    [Tooltip("Speed of door sliding")]
    public float slideSpeed = 5f;

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

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Assert.IsNotNull(rb, "Door3D requires a Rigidbody component.");

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
            rb.MovePosition(
                Vector3.MoveTowards(
                    transform.position,
                    isOpen ? openPosition : closedPosition,
                    slideSpeed * Time.deltaTime
                )
            );

            // Check if door has reached its destination
            if (
                Vector3.Distance(transform.position, isOpen ? openPosition : closedPosition)
                < 0.001f
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
