using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

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
    private FloppyDisk[] avatarKeys = new FloppyDisk[KeySize];
    public const int KeySize = 2;

    public void SetControllable(bool controllable = true)
    {
        this.controlling_ = controllable;
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

        az = GetComponent<AvatarZone>();
        Assert.IsNotNull(this.az);

        ai = GetComponent<AvatarInput>();
        Assert.IsNotNull(this.ai);

        sr.enabled = false;
    }

    public void MoveAvatarTo(Vector2 position)
    {
        transform.position = position;
    }

    public void ToggleSpriteRenderer(bool enable = true)
    {
        sr.enabled = enable;
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
        }
    }

    public void RemoveKey(FloppyDisk key)
    {
        if (avatarKeys.Contains(key))
        {
            var index = Array.IndexOf(avatarKeys, key);
            avatarKeys[index] = null;
        }
    }
}
