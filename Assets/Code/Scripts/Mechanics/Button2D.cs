using NUnit.Framework;
using UnityEngine;

public class Button2D : Button
{
    public Sprite pressedSprite;
    public Sprite unpressedSprite;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Assert.IsNotNull(
            spriteRenderer,
            $"{this.name} of {transform.parent.gameObject.name} has no sprite renderer."
        );
    }

    // todo: some sprite crap
    protected override void UnuseAnimation()
    {
        spriteRenderer.sprite = unpressedSprite;
    }

    protected override void UseAnimation()
    {
        spriteRenderer.sprite = pressedSprite;
    }
}
