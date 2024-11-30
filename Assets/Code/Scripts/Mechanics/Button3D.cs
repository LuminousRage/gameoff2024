using UnityEngine;

public class Button3D : Button
{
    protected override void UnuseAnimation()
    {
        transform.position += Vector3.forward * 0.1f;
    }

    protected override void UseAnimation()
    {
        transform.position += Vector3.back * 0.1f;
    }
}