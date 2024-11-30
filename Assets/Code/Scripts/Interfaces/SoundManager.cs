using System.Collections.Generic;
using UnityEngine;

public abstract class SoundManager : MonoBehaviour
{
    private bool enabled_ = false;

    public abstract void ToggleOn();
    public abstract void ToggleOff();

    public void Toggle()
    {
        if (this.enabled_)
            ToggleOff();
        else
            ToggleOn();
    }

    public bool SoundEnabled()
    {
        return enabled_;
    }
}
