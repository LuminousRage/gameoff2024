using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PlayerReacher : MonoBehaviour, IUsableSetter
{
    private Player player_;
    private List<IUsable> usable_ = new List<IUsable>();

    private SceneManager sm_;

    private Computer computer_;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player_ = this.transform.GetComponentInParent<Player>();
        Assert.IsNotNull(player_, "Unable to find Player script from PlayerReacher.");

        sm_ = GameObject.FindFirstObjectByType<SceneManager>();
        Assert.IsNotNull(sm_, "Unable to find SceneManager from Player.");
    }

    // Update is called once per frame
    void Update() { }

    public void UseUsable()
    {
        if (this.usable_.Count == 0)
        {
            return;
        }

        Debug.Log($"Using usable: {this.usable_}");
        this.usable_[0]?.Use(this.player_);
    }

    public void SetUsable(IUsable usable)
    {
        if (this.usable_.Contains(usable))
        {
            return;
        }
        Debug.Log($"Setting PlayerReacher usable to {usable}");
        this.usable_.Add(usable);
        return;
    }

    public void UnsetUsable(IUsable usable)
    {
        if (this.usable_.Contains(usable))
        {
            Debug.Log($"Unsetting PlayerReacher usable from {usable}");
            this.usable_.Remove(usable);
        }
    }

    public List<IUsable> SortUsablePriority()
    {
        return this.usable_;
    }

    void OnTriggerEnter(Collider c)
    {
        var usable = c.GetComponent<IUsable>();
        if (usable == null)
        {
            // Debug.Log("Collision occured with non IUsable.");
            return;
        }
        if (usable.IsCurrentlyUsable())
        {
            this.SetUsable(usable);
        }

        // computer is a usable, so no fear of early return
        var comp = c.GetComponent<Computer>();
        if (comp != null)
        {
            computer_ = comp;
        }
    }

    void OnTriggerExit(Collider c)
    {
        var usable = c.GetComponent<IUsable>();
        if (usable == null)
        {
            Debug.Log("Collision occured with non IUsable.");
            return;
        }

        this.UnsetUsable(usable);

        var comp = c.GetComponent<Computer>();
        if (comp != null)
        {
            computer_ = null;
        }
    }

    public Computer GetComputer()
    {
        return computer_;
    }
}
