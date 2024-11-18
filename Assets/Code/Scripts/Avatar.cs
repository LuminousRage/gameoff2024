using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Avatar : MonoBehaviour
{
    public InputAction move;

    [Range(0, 1)]
    public float movementDelay = 0.1f;

    private float timer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        move.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        MovementHandler();
    }

    void MovementHandler()
    {
        timer += Time.deltaTime;

        if (timer > movementDelay)
        {
            var directions = move.ReadValue<Vector2>();
            // Move the player
            transform.position += new Vector3(
                Mathf.Ceil(directions.x),
                Mathf.Ceil(directions.y),
                0
            );
            timer = 0;
        }
    }
}
