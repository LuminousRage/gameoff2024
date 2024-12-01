using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class LaserDoor : MonoBehaviour
{
    private Transform[] children;

    // this is assigned at runtime through the floppy disk
    [HideInInspector]
    public Color color;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        children = GetComponentsInChildren<Transform>();
        children = children.Where(child => child.name.StartsWith("Door")).ToArray();
        Assert.IsTrue(children.Length == 3, "LaserDoor children should be 3");

        children
            .ToList()
            .ForEach(child =>
            {
                Assert.IsNotNull(
                    child.GetComponent<SpriteRenderer>(),
                    "Laser Door Children should have SR"
                );
            });
    }

    public void UpdateLaserCollision(Avatar avatarWithFloppy)
    {
        var avatarId = avatarWithFloppy.number;
        var avatarLayer = avatarWithFloppy.gameObject.layer;

        children
            .ToList()
            .ForEach(child =>
            {
                var avatarHasRights = child.name.Contains(avatarId.ToString());
                UpdateSprite(child, avatarHasRights);
            });
    }

    void UpdateSprite(Transform child, bool hasRights)
    {
        // Idk  make the sprite opaque or smth
        var sr = child.GetComponent<SpriteRenderer>();
        Assert.IsNotNull(sr, "LaserDoor child should have a SpriteRenderer");

        var opacity = hasRights ? .5f : 1f;
        sr.color = new Color(1f, 1f, 1f, opacity);
    }
}
