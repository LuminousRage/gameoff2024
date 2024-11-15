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
        bool back = Input.GetKey(KeyCode.S);
        bool forward = Input.GetKey(KeyCode.W);
        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);

        Vector3 direction = Vector3.zero;
        if (forward) {direction += Vector3.forward;}
        if (back) {direction += Vector3.back;}
        if (left) {direction += Vector3.left;}
        if (right) {direction += Vector3.right;}

        rb_.AddForce(acceleration * direction.normalized, ForceMode.Acceleration);
    }
}
