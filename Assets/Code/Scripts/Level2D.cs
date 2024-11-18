using UnityEngine;

public class Level2D : MonoBehaviour, IControllable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update() { }

    private bool controllable = false;

    public void EnterFrom(Computer c) { }

    public void SetControllable(bool controllable = true)
    {
        this.controllable = controllable;
    }
}
