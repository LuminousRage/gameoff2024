using UnityEngine;

public class Zone : MonoBehaviour
{
    [Range(0, 9)]
    public int zone = 0;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var Avatar = other.GetComponent<Avatar>();
            if (Avatar.GetZone() != zone)
            {
                Avatar.SetZone(zone);
                Debug.Log($"Avatar entered zone {zone}");
            }
        }
    }
}
