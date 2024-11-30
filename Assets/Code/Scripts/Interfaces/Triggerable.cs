using UnityEngine;

public abstract class Triggerable : MonoBehaviour
{
    // how many trigger signals are needed to activate the triggerable
    public int signalCount = 1;
    protected int currentSignalCount = 0;

    public abstract void Trigger();

    public abstract void Untrigger();

    protected bool CanTrigger() => currentSignalCount >= signalCount;

    protected void UpdateSignalCount(bool triggered)
    {
        if (triggered)
        {
            currentSignalCount++;
        }
        else
        {
            currentSignalCount--;
        }

        if (currentSignalCount < 0)
        {
            currentSignalCount = 0;
        }

        if (currentSignalCount >= signalCount)
        {
            currentSignalCount = signalCount;
        }
    }
}
