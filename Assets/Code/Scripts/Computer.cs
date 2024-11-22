using Globals;
using UnityEngine;
using UnityEngine.Assertions;

public class Computer : MonoBehaviour, IUsable
{
    public Level2D level;
    public Globals.Zone zone;

    [Range(1, 8)]
    public byte avatar;

    private Camera renderCamera_;

    private GameObject watcher_;

    private SceneManager sceneManager_;

    public void Use(Player p)
    {
        sceneManager_.SetFocus(this);
    }

    public string GetUsableLabel()
    {
        return "Computer";
    }

    public Transform GetWatcherTransform()
    {
        return watcher_.transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Assert.IsNotNull(this.level);
        NUnit.Framework.Assert.IsInstanceOf<Level2D>(this.level);

        renderCamera_ = GetComponentInChildren<Camera>();
        Assert.IsNotNull(renderCamera_, "Unable to find render camera in Computer.");

        watcher_ = this.transform.Find("Watcher").gameObject;
        Assert.IsNotNull(watcher_, "Unable to find watcher in Computer.");

        sceneManager_ = FindFirstObjectByType<SceneManager>();
        Assert.IsNotNull(sceneManager_, "Unable to find SceneManager from Computer.");

        renderCamera_.transform.position = level.transform.position - 2 * level.transform.forward;
        renderCamera_.transform.rotation = level.transform.rotation;
    }

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