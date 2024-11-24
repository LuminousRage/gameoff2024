using UnityEngine;

public interface IUsable
{
    public void Use(IControllable p);
    public string GetUsableLabel();
    public string GetActionLabel()
    {
        return "Use";
    }
}
