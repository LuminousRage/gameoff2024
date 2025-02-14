using UnityEngine;
using UnityEngine.Assertions;

public class ComputerSoundManager : MonoBehaviour
{
    // Parameters

    [Range(0, 1)]
    public float fanVolume = 0.1f;

    [Range(0, 1)]
    public float zoneVolume = 0.8f;
    public float crossFadeRate = 0.3f;

    // Sources

    [Tooltip("The sound that plays when the computer is turned on.")]
    public AudioSource computerOnSource;

    [Tooltip("The sound that plays when the computer is turned off.")]
    public AudioSource computerOffSource;

    public AudioSource fanStart;
    public AudioSource fanLoop;
    public AudioSource fanStop;

    public AudioSource zoneALoop;
    public AudioSource zoneBLoop;
    public AudioSource zoneCLoop;
    public AudioSource brokenLoop;

    private Computer computer_;

    public void Start()
    {
        computer_ = GetComponent<Computer>();

        fanStart.volume = fanVolume;
        fanStop.volume = fanVolume;
        zoneALoop.volume = 0;
        zoneBLoop.volume = 0;
        zoneCLoop.volume = 0;
        brokenLoop.volume = 0;
        fanLoop.volume = 0;
        zoneALoop.Play();
        zoneBLoop.Play();
        zoneCLoop.Play();
        brokenLoop.Play();
        fanLoop.Play();
    }

    public void ToggleOn()
    {
        try
        {
            computerOnSource.Play();
            fanStart.Play();
            fanLoop.PlayScheduled(fanStart.clip.length);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Unable to play computer on sound. Exception: {e}");
        }
    }

    void Update()
    {
        if (!computer_.quad_.activeSelf) {
            return;
        }
        var avAudio = computer_.avatarObj.GetComponent<AvatarAudio>();
        var amount = crossFadeRate * Time.deltaTime;
        avAudio.zoneA = Mathf.Clamp(avAudio.zoneA + amount * (computer_.zone == Globals.Zone.A ? 1 : -1), 0, 1);
        avAudio.zoneB = Mathf.Clamp(avAudio.zoneB + amount * (computer_.zone == Globals.Zone.B ? 1 : -1), 0, 1);
        avAudio.zoneC = Mathf.Clamp(avAudio.zoneC + amount * (computer_.zone == Globals.Zone.C ? 1 : -1), 0, 1);
        avAudio.zoneBroken = Mathf.Clamp(avAudio.zoneBroken + amount * (computer_.zone == Globals.Zone.Broken ? 1 : -1) , 0, 1);
        zoneALoop.volume = avAudio.zoneA * zoneVolume;
        zoneBLoop.volume = avAudio.zoneB * zoneVolume;
        zoneCLoop.volume = avAudio.zoneC * zoneVolume;
        brokenLoop.volume = avAudio.zoneBroken * zoneVolume;
    }

    public void ToggleOff()
    {
        try
        {
            zoneALoop.volume = 0;
            zoneBLoop.volume = 0;
            zoneCLoop.volume = 0;
            brokenLoop.volume = 0;
            fanLoop.volume = 0;
            computerOffSource.Play();
            fanStart.Stop();
            fanStop.Play();
        }
        catch
        {
            Debug.LogError("Unable to play computer off sound.");
        }
    }
}
