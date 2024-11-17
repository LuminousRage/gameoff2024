using UnityEngine;
using UnityEngine.InputSystem;

public class MouseManager : MonoBehaviour
{
    public Vector2 delta { get; private set; } = Vector2.zero;

    [Range(1f, 10f)]
    public float sensitivity = 5f;

    private bool locked = false;

    public Vector2 GetScaledDelta()
    {
        // Use the sensitivity value
        return this.delta * this.sensitivity;
    }

    public void Toggle()
    {
        if (this.locked)
            this.Unlock();
        else
            this.Lock();
    }

    public void Lock()
    {
        if (this.locked)
            Debug.LogWarning($"Attempted to lock the MouseManager when it it already locked.");
        else
        {
            Debug.Log("Locking the cursor.");
        }

        Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
        Cursor.visible = true;

        this.locked = true;

        return;
    }

    public void Unlock()
    {
        if (!this.locked)
            Debug.LogWarning($"Attempted to unlock the MouseManager when it it already unlocked.");
        else
        {
            Debug.Log("Unlocking the cursor.");
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        this.locked = false;
    }

    void Start()
    {
        this.Lock();
    }

    // Update is called once per frame
    void Update()
    {
        var newDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        this.delta = newDelta;
    }

    private Vector2 GetMousePosition()
    {
        return Mouse.current.position.ReadValue();
    }
}
