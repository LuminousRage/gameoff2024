
using System;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeIn : MonoBehaviour
{
    public Image cover;
    public float coverDuration = 2;
    public float coverAlphaPower=3;
    private float lastTime = 0;

    void Update()
    {
        if (lastTime <= coverDuration) {
            var tempColor = cover.color;
            tempColor.a = (float) (1 - Math.Min(Math.Pow(Time.timeSinceLevelLoad/coverDuration,coverAlphaPower),1));
            cover.color = tempColor;
            lastTime = Time.timeSinceLevelLoad;
        }
    }
}
