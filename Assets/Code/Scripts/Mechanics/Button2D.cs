using UnityEngine;
using UnityEngine.Assertions;

public class Button2D : Button
{
    public Sprite pressedSprite;
    public Sprite unpressedSprite;

    private SpriteRenderer spriteRenderer;

    public AudioSource buttonPressSound;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Assert.IsNotNull(
            spriteRenderer,
            $"{this.name} of {transform.parent.gameObject.name} has no sprite renderer."
        );
    }

    protected override void UnuseAnimation()
    {
        spriteRenderer.sprite = unpressedSprite;
    }

    protected override void UseAnimation()
    {
        spriteRenderer.sprite = pressedSprite;
        buttonPressSound?.Play();
    }
}
