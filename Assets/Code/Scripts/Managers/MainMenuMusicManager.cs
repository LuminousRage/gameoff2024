using UnityEngine;

public class MainMenuMusicManager : MonoBehaviour
{
    public AudioSource musicStart;
    public AudioSource musicLoop;
    public AudioSource musicEnd;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicStart.Play();

        double startTime = AudioSettings.dspTime;
        musicLoop.PlayScheduled(startTime + musicStart.clip.length);
    }
}
