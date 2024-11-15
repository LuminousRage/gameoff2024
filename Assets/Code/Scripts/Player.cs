using NUnit.Framework;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb_;
    public float maxMoveSpeed = 1;
    public float acceleration = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb_ = GetComponent<Rigidbody>();
        Assert.NotNull(rb_, "Unable to find Rigidbody on a Player instance.");
    }

    // Update is called once per frame
    void Update() { }

    void FixedUpdate()
    {
        bool down = Input.GetKeyDown(KeyCode.S);
        bool up = Input.GetKey(KeyCode.W);
        bool left = Input.GetKeyDown(KeyCode.A);
        bool right = Input.GetKeyDown(KeyCode.D);

        if (up)
        {
            rb_.AddForce(acceleration * Vector3.one, 0);
        }
    }
}
