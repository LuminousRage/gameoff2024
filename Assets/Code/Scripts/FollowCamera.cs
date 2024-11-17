using UnityEngine;
using UnityEngine.Assertions;

public class FollowCamera : MonoBehaviour
{
    public GameObject _followee = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Assert.IsNotNull(
            this._followee,
            "A followee has not been assigned to the PlayerCamera. Please set this in the inspector."
        );
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = _followee.transform.position;
        this.transform.rotation = _followee.transform.rotation;
    }
}
