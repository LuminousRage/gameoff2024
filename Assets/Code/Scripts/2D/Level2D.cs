using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class Level2D : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transformPosition_ = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update() { }

    bool entered_ = false;
    Vector2 transformPosition_;

    public void EnterFrom(Computer c)
    {
        if (c == null)
        {
            Debug.LogError("Attempted to enter level from a null Computer. Continuing anyway.");
            return;
        }
        var (avatar, zone) = GetAndValidateAvatarAndZone(c);
        if (avatar == null || zone == null)
        {
            Debug.LogError("Failed to enter level. Exiting.");
            return;
        }

        var avatarLastZone = avatar.GetZone();
        var spawnPoint = zone.spawnPoints.First(sp => sp.avatarId == avatar.number).spawnPoint;
        // Avatar has never entered, or was previously exited out in a different zone

        if (avatarLastZone != zone.number || avatarLastZone == null)
        {
            Debug.Log($"Respawning avatar to ${spawnPoint + transformPosition_}");

            avatar.MoveAvatarTo(spawnPoint + transformPosition_);
            // This honestly just need to happen once when the avatar is first created, but it doesn't hurt to keep it
            avatar.ToggleSpriteRenderer(true);
            // Zone resetting is automatically done once the avatar is detected in the new zone
        }

        avatar.SetControllable(true);

        entered_ = true;
    }

    (Avatar, CollisionZone) GetAndValidateAvatarAndZone(Computer c)
    {
        var avatars = GetComponentsInChildren<Avatar>().Where(a => a.number == c.avatar).ToList();
        var zones = GetComponentsInChildren<CollisionZone>()
            .Where(z => z.number == c.zone)
            .ToList();
        (Avatar, CollisionZone) badResult = (null, null);

        if (avatars.Count != 1 || zones.Count != 1)
        {
            Debug.LogError(
                "Expected 1 avatar and 1 zone for entering a level."
                    + $"Found {avatars.Count} avatars and {zones.Count} zones."
            );

            return badResult;
        }

        var avatar = avatars[0];
        var zone = zones[0];

        if (!zone.spawnPoints.Any(sp => sp.avatarId == avatar.number))
        {
            Debug.LogError(
                $"Avatar {avatar.number} not found in spawn points of zone {zone.number}."
            );

            return badResult;
        }

        return (avatar, zone);
    }

    public void Exit()
    {
        if (!entered_)
        {
            Debug.LogError("Attempted to exit level without entering. Exiting anyway.");
        }

        GetComponentsInChildren<Avatar>().ToList().ForEach(a => a.SetControllable(false));
        entered_ = false;
    }
}