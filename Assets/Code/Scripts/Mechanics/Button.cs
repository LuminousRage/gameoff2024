using UnityEngine;
using UnityEngine.Assertions;

public abstract class Button : MonoBehaviour, IUsable
{
    public Triggerable triggerable;
    bool isPressed = false;

    public float activeForSeconds = 1.0f;

    protected UseState state = UseState.Usable;

    public string inputKey = "F";

    void Start()
    {
        Assert.IsNotNull(
            triggerable,
            $"{this.name} of {transform.parent.gameObject.name} has no triggerable assigned."
        );
    }

    IUsableSetter GetUsableSetter(GameObject obj)
    {
        var ai = obj.GetComponent<AvatarInput>(); // 2d case
        var pr = obj.GetComponent<PlayerReacher>(); // 3d case

        Assert.IsFalse(ai == null && pr == null, "Both AvatarInput and PlayerReacher are null.");
        // There can be only one AvatarInput or PlayerReacher
        Assert.IsFalse(
            ai != null && pr != null,
            "Both AvatarInput and PlayerReacher are not null."
        );

        return ai != null ? ai : pr;
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
        if (!isPressed)
        {
            isPressed = true;
            triggerable.Trigger();
            UseAnimation();
            state = UseState.Activated;
            Invoke("ResetButton", activeForSeconds);
        }
    }

    void ResetButton()
    {
        isPressed = false;
        triggerable.Untrigger();
        UnuseAnimation();
        state = UseState.Usable;
    }

    protected abstract void UseAnimation();
    protected abstract void UnuseAnimation();

    public enum UseState
    {
        Usable,
        Activated,
    }

    public string GetUsableLabel() => "Button";

    public string GetActionLabel() => "Press";

    public string GetUsePrompt()
    {
        switch (state)
        {
            case UseState.Usable:
                return $"{GetActionLabel()} {GetUsableLabel()}";
            case UseState.Activated:
                return $"Button is active";
            default:
                return $"{GetActionLabel()} {GetUsableLabel()}";
        }
    }

    public string GetKeyLabel()
    {
        switch (state)
        {
            case UseState.Usable:
                return inputKey;
            case UseState.Activated:
                return "";
            default:
                return inputKey;
        }
    }
}
