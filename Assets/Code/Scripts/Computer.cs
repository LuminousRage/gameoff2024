using UnityEngine;

public class Computer : MonoBehaviour, IUsable
{
    public void Use(Player p)
    {
        Debug.Log("Using Computer (TODO: IMPLEMENT)");
    }

    public string GetUsableLabel()
    {
        return "Computer";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update() { }

    void OnTriggerEnter(Collider c)
    {
        var reacher = c.GetComponent<PlayerReacher>();
        if (reacher == null)
        {
            Debug.Log("Collision occured with non PlayerReacher.");
            return;
        }

        reacher.SetUsable(this);
    }

    void OnTriggerExit(Collider c)
    {
        var reacher = c.GetComponent<PlayerReacher>();
        if (reacher == null)
        {
            Debug.Log("Collision occured with non PlayerReacher.");
            return;
        }

        reacher.UnsetUsable(this);
    }
}
