using UnityEngine;
using UnityEngine.Assertions;

public class PressurePad : MonoBehaviour
{
    public Triggerable triggerable;

    public Sprite onSprite;
    public Sprite offSprite;

    private SpriteRenderer spriteRenderer;

    private int thingsOnPad = 0;

    void Start()
    {
        Assert.IsNotNull(triggerable);
        spriteRenderer = GetComponent<SpriteRenderer>();
        Assert.IsNotNull(
            spriteRenderer,
            $"{this.name} of {transform.parent.gameObject.name} has no sprite renderer."
        );

        spriteRenderer.sprite = offSprite;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (thingsOnPad == 0)
            {
                spriteRenderer.sprite = onSprite;
                triggerable.Trigger();
            }
            thingsOnPad++;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            thingsOnPad--;
            if (thingsOnPad == 0)
            {
                spriteRenderer.sprite = offSprite;
                triggerable.Untrigger();
            }
        }
    }
}
