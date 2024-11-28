using UnityEngine;

public class Door : MonoBehaviour, ITriggerable
{
    private bool isOpen = false;

    public void Trigger()
    {
        if (!isOpen)
        {
            isOpen = true;
            Debug.Log($"Door {this.GetInstanceID()} opened");
        }
    }

    public void Untrigger()
    {
        if (isOpen)
        {
            isOpen = false;
            Debug.Log($"Door {this.GetInstanceID()} closed");
        }
    }
}
