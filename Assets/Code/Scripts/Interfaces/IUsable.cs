using UnityEngine;

public interface IUsable
{
    // Okay I might need a fact check later but I don't think we'd ever need to pass in a player to Use
    public void Use(IControllable p);
    public string GetUsableLabel();
    public string GetActionLabel() => "Use";
}
