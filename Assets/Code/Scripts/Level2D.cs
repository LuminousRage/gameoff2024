using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class Level2D : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update() { }

    bool entered_ = false;

    public void EnterFrom(Computer c)
    {
        if (c == null)
        {
            Debug.LogError("Attempted to enter level from a null Computer. Continuing anyway.");
            return;
        }

        var avatars = GetComponentsInChildren<Avatar>().Where(a => a.number == c.avatar).ToList();

        if (!avatars.Any())
        {
            Debug.LogError($"No avatars found with avatar number {c.avatar}.");
            return;
        }
        else if (avatars.Count != 1)
        {
            Debug.LogError($"Expected 1 avatar for entering a level. Found {avatars.Count}.");
            return;
        }

        avatars[0].SetControllable(true);
        entered_ = true;
    }

    public void Exit()
    {
        if (!entered_)
        {
            return;
        }

        GetComponentsInChildren<Avatar>().ToList().ForEach(a => a.SetControllable(false));
    }
}
