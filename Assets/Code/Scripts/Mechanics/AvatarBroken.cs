using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

public class AvatarBroken : MonoBehaviour, IUsable
{
    public EventTrigger.TriggerEvent trigger;
    bool isSpeaking = false;


    public string inputKey = "F";

    void Start()
    {
        Assert.IsNotNull(
            trigger,
            $"{this.name} of {transform.parent.gameObject.name} has no triggerable assigned."
        );
    }

    IUsableSetter GetUsableSetter(GameObject obj)
    {
        var ai = obj.GetComponent<AvatarInput>(); // 2d case

        Assert.IsFalse(ai == null, "AvatarInput is null.");

        return ai;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var usableSetter = GetUsableSetter(collision.gameObject);
            usableSetter.SetUsable(this);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var usableSetter = GetUsableSetter(collision.gameObject);
            usableSetter.UnsetUsable(this);
        }
    }

    public void Use(IControllable p)
    {
        if (!isSpeaking)
        {
                isSpeaking = true;
            BaseEventData eventData = new BaseEventData(EventSystem.current);
            eventData.selectedObject = this.gameObject;
            trigger.Invoke(eventData);
        }
    }

    public void resetSpeaking() {
        isSpeaking = false;
    }



    public string GetUsableLabel() => "???";

    public string GetActionLabel() => "Speak to";

    public string GetKeyLabel()
    {
        return inputKey;
    }
}
