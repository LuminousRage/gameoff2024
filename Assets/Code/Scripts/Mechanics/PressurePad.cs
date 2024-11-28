using UnityEngine;
using UnityEngine.Assertions;

public class PressurePad : MonoBehaviour
{
    public Triggerable triggerable;

    void Start()
    {
        Assert.IsNotNull(triggerable);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            triggerable.Trigger();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            triggerable.Untrigger();
        }
    }
}
