using UnityEngine;

public class Button3D : Button
{

    public AudioSource buttonPressSound;

    protected override void UnuseAnimation()
    {
        transform.position += transform.forward * 0.1f;
    }

    protected override void UseAnimation()
    {
        transform.position -= transform.forward * 0.1f;
        buttonPressSound?.Play();
    }
}
