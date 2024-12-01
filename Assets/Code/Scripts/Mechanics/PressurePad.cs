using UnityEngine;
using UnityEngine.Assertions;

public class PressurePad : MonoBehaviour
{
    public Triggerable triggerable;

    public Sprite onSprite;
    public Sprite offSprite;

    private SpriteRenderer spriteRenderer;

    private int thingsOnPad = 0;
    public AudioSource padPressSound;
    public AudioSource padUnPressSound;

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
                padPressSound?.Play();
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
                padUnPressSound?.Play();
                spriteRenderer.sprite = offSprite;
                triggerable.Untrigger();
            }
        }
    }
}
