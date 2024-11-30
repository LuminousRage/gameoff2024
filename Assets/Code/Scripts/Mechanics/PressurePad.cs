using UnityEngine;
using UnityEngine.Assertions;

public class PressurePad : MonoBehaviour
{
    public Triggerable triggerable;

    private int thingsOnPad = 0;

    void Start()
    {
        Assert.IsNotNull(triggerable);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (thingsOnPad == 0)
            {
                triggerable.Trigger();
            }
            thingsOnPad++;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (thingsOnPad == 0)
            {
                triggerable.Untrigger();
            }
            thingsOnPad--;
        }
    }
}
