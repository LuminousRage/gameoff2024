using NUnit.Framework;
using UnityEngine;

public class PlayerReacher : MonoBehaviour
{
    private Player player_;
    private IUsable usable_;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player_ = this.transform.GetComponentInParent<Player>();
        Assert.IsNotNull(player_, "Unable to find Player script from PlayerReacher.");
    }

    // Update is called once per frame
    void Update() { }

    public void UseUsable()
    {
        Debug.Log($"Using usable: {this.usable_}");
        this.usable_?.Use(this.player_);
    }

    public void SetUsable(IUsable usable)
    {
        Debug.Log($"Setting usable to {usable}");

        this.usable_ = usable;
    }

    public void UnsetUsable(IUsable usable)
    {
        if (this.usable_ == usable)
        {
            Debug.Log($"Unsetting usable from {usable}");
            this.usable_ = null;
        }
    }
}
