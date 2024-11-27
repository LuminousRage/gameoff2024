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
    private Camera renderCamera_;

    public const int KeySize = 2;

    public void SetControllable(bool controllable = true)
    {
        renderCamera_.gameObject.SetActive(controllable);
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

        renderCamera_ = GetComponentInChildren<Camera>();
        Assert.IsNotNull(renderCamera_, "Unable to find render camera in Avatar.");

        var cameraOffset = 2 * level.transform.forward;
        renderCamera_.transform.position = level.transform.position - cameraOffset;
        renderCamera_.transform.rotation = level.transform.rotation;

        SetActive(false);
    }

    public void MoveAvatarTo(Vector2 position)
    {
        transform.position = position;
    }

    public void SetActive(bool enable = true)
    {
        // We can't set the avatars to inactive because the lookup table will be broken
        // instead let's just turn on/off the interactable parts of the avatar
        sr.enabled = enable;
        rb.simulated = enable;
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
            key.SetFloppyDiskTransform(emptyIndex);
            level.TellOtherComputersToRenderGhostDisks(number, key.GetComputer(), emptyIndex, true);
        }
    }

    public void RemoveKey(FloppyDisk key)
    {
        if (avatarKeys.Contains(key))
        {
            var index = Array.IndexOf(avatarKeys, key);
            level.TellOtherComputersToRenderGhostDisks(number, key.GetComputer(), index, false);
            avatarKeys[index] = null;
        }
    }
}
