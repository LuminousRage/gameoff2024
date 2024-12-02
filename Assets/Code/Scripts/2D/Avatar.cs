using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Avatar : MonoBehaviour, IControllable
{
    private Level2D level;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    [Range(1, 8)]
    public byte number = 1;

    private bool controlling_ = false;

    public AvatarZone az;
    public AvatarInput ai;
    public SpriteRenderer leftHand;
    public SpriteRenderer rightHand;

    public Sprite keyA;
    public Sprite keyB;
    public Sprite keyC;
    private FloppyDisk[] avatarKeys = new FloppyDisk[KeySize];
    private Camera renderCamera_;

    // Whether the avatar has been entered before through a computer
    [HideInInspector]
    public bool hasEntered = false;

    public const int KeySize = 2;

    public void SetControllable(bool controllable = true)
    {
        this.controlling_ = controllable;
        if (!controllable)
        {
            ai.animator.SetBool("IsWalking", false);
        }
        if (ai != null)
        {
            ai.SetInputEnable(controllable);
        }
    }

    public bool GetControllable()
    {
        return this.controlling_;
    }

    void Start()
    {
        // for debug only
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (scene.name.Contains("Demo"))
        {
            SetControllable(true);
        }

        sr = GetComponent<SpriteRenderer>();
        Assert.IsNotNull(this.sr);

        level = GetComponentInParent<Level2D>();
        Assert.IsNotNull(this.level);

        rb = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(this.rb);
        rb.bodyType = RigidbodyType2D.Static;

        renderCamera_ = GetComponentInChildren<Camera>();
        Assert.IsNotNull(renderCamera_, "Unable to find render camera in Avatar.");

        SetRenderCamera(false);
        SetRenderCamera(PlayerPrefs.GetInt("ContinueLevel", 0) == level.levelOrder);
    }

    public Level2D GetLevel()
    {
        return level;
    }

    public Rigidbody2D GetRigidbody()
    {
        return rb;
    }

    public FloppyDisk[] GetKeys()
    {
        return avatarKeys;
    }

    public bool IsKeysFull()
    {
        var keysCount = avatarKeys.Count(k => k != null);
        return keysCount >= KeySize;
    }

    public void AddKey(FloppyDisk key)
    {
        if (!avatarKeys.Contains(key))
        {
            var emptyIndex = Array.IndexOf(avatarKeys, null);
            if (emptyIndex == -1)
            {
                Debug.LogError("Avatar already has max keys.");
            }
            avatarKeys[emptyIndex] = key;
            var hand = leftHand;
            if (emptyIndex == 1)
            {
                hand = rightHand;
            }

            switch (key.floppyDiskID)
            {
                case Globals.FloppyDiskID.A:
                    hand.sprite = keyA;
                    break;
                case Globals.FloppyDiskID.B:
                    hand.sprite = keyB;
                    break;
                case Globals.FloppyDiskID.C:
                    hand.sprite = keyC;
                    break;
            }

            key.SetFloppyDiskTransform(emptyIndex);
            level.TellOtherComputersToRenderGhostDisks(
                number,
                key.GetComputer(),
                emptyIndex,
                key.floppyDiskID
            );
        }
    }

    public void RemoveKey(FloppyDisk key)
    {
        if (avatarKeys.Contains(key))
        {
            var index = Array.IndexOf(avatarKeys, key);
            level.TellOtherComputersToRenderGhostDisks(number, key.GetComputer(), index);
            avatarKeys[index] = null;

            var hand = leftHand;
            if (index == 1)
            {
                hand = rightHand;
            }
            hand.sprite = null;
        }
    }

    public void SetRenderCamera(bool enable = true)
    {
        renderCamera_.enabled = enable;
    }
}
