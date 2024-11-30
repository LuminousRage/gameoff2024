using UnityEngine;

public abstract class Door : Triggerable
{
    protected bool isOpen = false;

    public override void Trigger()
    {
        UpdateSignalCount(true);

        if (!isOpen && CanTrigger())
        {
            isOpen = true;
            OpenSesame();
            Debug.Log($"Door {this.GetInstanceID()} opened");
        }
    }

    public override void Untrigger()
    {
        UpdateSignalCount(false);

        if (isOpen && !CanTrigger())
        {
            isOpen = false;
            CloseSesame();
            Debug.Log($"Door {this.GetInstanceID()} closed");
        }
    }

    public abstract void OpenSesame();

    public abstract void CloseSesame();
}
