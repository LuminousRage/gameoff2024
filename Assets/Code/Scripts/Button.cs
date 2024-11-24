using NUnit.Framework;
using UnityEngine;

public class Button : MonoBehaviour, IUsable
{
    // public Door door;
    bool isPressed = false;

    public float activeForSeconds = 1.0f;

    void Start() { }

    void FixedUpdate() { }

    IUsableSetter GetUsableSetter(GameObject obj)
    {
        var ai = obj.GetComponent<AvatarInput>(); // 2d case
        var pr = obj.GetComponent<PlayerReacher>(); // 3d case

        Assert.IsFalse(ai == null && pr == null, "Both AvatarInput and PlayerReacher are null.");
        // There can be only one AvatarInput or PlayerReacher
        Assert.IsFalse(
            ai != null && pr != null,
            "Both AvatarInput and PlayerReacher are not null."
        );

        return ai != null ? ai : pr;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var usableSetter = GetUsableSetter(collision.gameObject);
            usableSetter.SetUsable(this);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var usableSetter = GetUsableSetter(collision.gameObject);
            usableSetter.UnsetUsable(this);
        }
    }

    public void Use(IControllable p)
    {
        throw new System.NotImplementedException();
    }

    public string GetUsableLabel()
    {
        throw new System.NotImplementedException();
    }
}
