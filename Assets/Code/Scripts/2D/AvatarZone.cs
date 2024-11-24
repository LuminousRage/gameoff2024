using UnityEngine;
using UnityEngine.Assertions;

public class AvatarZone : MonoBehaviour
{
    private Avatar avatar;

    private Globals.Zone? zone;

    void Start()
    {
        avatar = GetComponent<Avatar>();
        Assert.IsNotNull(this.avatar);
    }

    public void SetZone(Globals.Zone newZone)
    {
        if (avatar.GetControllable())
        {
            Debug.Log($"Changing Avatar zone from {zone} to {newZone}");
            avatar.GetLevel().UpdatePlayerToComputer(avatar.number, newZone, zone);
            zone = newZone;
        }
    }

    public Globals.Zone? GetZone()
    {
        return zone;
    }
}
