using System;

public interface IUsable
{
    public void Use(IControllable p);
    public bool IsCurrentlyUsable() => true;

    public string GetUsableLabel();
    public string GetActionLabel() => "Use";
    public string GetKeyLabel() => "E";

    public string GetUsePrompt() => $"{GetActionLabel()} {GetUsableLabel()}";
}
