using UnityEngine;

public interface IUsable
{
    public void Use(Player p);
    public string GetUsableLabel();
}