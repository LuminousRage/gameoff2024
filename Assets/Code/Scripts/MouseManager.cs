using UnityEngine;
using UnityEngine.InputSystem;

public class MouseManager : MonoBehaviour
{
    public Vector2 delta { get; private set; } = Vector2.zero;
    private Vector2 mousePos_;

    private bool locked;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.mousePos_ = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        var newPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        var newDelta = newPos - this.mousePos_;
        this.delta = newDelta;

        this.mousePos_ = newPos;

        this.ResetMouse();
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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        return;
    }

    public void Unlock()
    {
        if (!this.locked)
            Debug.LogWarning($"Attempted to unlock the MouseManager when it it already unlocked.");
        return;
    }

    private void ResetMouse()
    {
        if (!this.locked)
            return;

        var middle = new Vector2(Screen.width / 2, Screen.height / 2);
        Mouse.current.WarpCursorPosition(middle);
    }
}
