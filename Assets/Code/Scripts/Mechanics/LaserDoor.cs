using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class LaserDoor : MonoBehaviour
{
    private Transform[] children;

    private BoxCollider2D c;

    // this is assigned at runtime through the floppy disk
    [HideInInspector]
    public Color color = Color.white;

    private SpriteRenderer laserSpriteRenderer;

    private bool startCalled = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        children = GetComponentsInChildren<Transform>();
        children = children.Where(child => child.name.StartsWith("Door")).ToArray();
        Assert.IsTrue(children.Length == 3, "LaserDoor children should be 3");

        var laser = transform.Find("LaserStripes");
        Assert.IsNotNull(laser, "LaserDoor should have a LaserStripes child");
        laserSpriteRenderer = laser.GetComponent<SpriteRenderer>();
        laserSpriteRenderer.color = color;

        c = GetComponent<BoxCollider2D>();
        Assert.IsNotNull(c, "LaserDoor should have a BoxCollider2D");

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

    public void UpdateLaserCollision(Avatar avatarWithFloppy, bool avatarHasRights)
    {
        Debug.Log(
            $"Updating laser collision for {avatarWithFloppy} with rights: {avatarHasRights}"
        );
        var avatarIdStr = avatarWithFloppy.number.ToString();

        if (!startCalled)
        {
            Start();
            startCalled = true;
        }

        Physics2D.IgnoreCollision(
            c,
            avatarWithFloppy.GetComponent<BoxCollider2D>(),
            avatarHasRights
        );

        children
            .ToList()
            .ForEach(child =>
            {
                var layerHasRights = child.name.Contains(avatarIdStr);

                UpdateSprite(child, layerHasRights);
            });
    }

    void UpdateSprite(Transform child, bool hasRights)
    {
        // Idk  make the sprite opaque or smth
        var sr = child.GetComponent<SpriteRenderer>();
        Assert.IsNotNull(sr, "LaserDoor child should have a SpriteRenderer");

        var outsideOpacity = hasRights ? .5f : 1f;
        sr.color = new Color(1f, 1f, 1f, outsideOpacity);
    }
}
