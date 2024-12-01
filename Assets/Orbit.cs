using UnityEngine;

public class Orbit : MonoBehaviour
{
    /* the object to orbit */
    public Transform target;

    /* speed of orbit (in degrees/second) */
    public float speed;

    public void Update()
    {
        if (target != null) {
            transform.RotateAround(target.position, Vector3.forward+Vector3.up*0.02f, speed * Time.deltaTime);
        }
    }
}