using UnityEngine;

public abstract class Door : Triggerable
{
    protected bool isOpen = false;

    public override void Trigger()
    {
        if (!isOpen)
        {
            isOpen = true;
            OpenSesame();
            Debug.Log($"Door {this.GetInstanceID()} opened");
        }
    }

    public override void Untrigger()
    {
        if (isOpen)
        {
            isOpen = false;
            CloseSesame();
            Debug.Log($"Door {this.GetInstanceID()} closed");
        }
    }

    public abstract void OpenSesame();

    public abstract void CloseSesame();
}
