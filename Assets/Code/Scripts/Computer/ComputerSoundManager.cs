using UnityEngine;
using UnityEngine.Assertions;

public class ComputerSoundManager : SoundManager
{
    // Parameters
    [Min(0)]
    public float zoneAudioDelaySeconds = 1;

    [Range(0, 1)]
    public float fanVolume = 0.1f;

    [Range(0, 1)]
    public float zoneVolume = 0.8f;

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

    // The zone audio
    private AudioSource computerZoneAudioSource = null;

    private float originalVolume = 0;

    public void Start()
    {
        computer_ = GetComponent<Computer>();

        Assert.IsNotNull(
            this.computer_,
            "Unable to find Computer in parent of ComputerSoundManager."
        );

        Assert.IsNotNull(
            this.computerOnSource,
            "Unable to find on button audio source in Computer."
        );

        Assert.IsNotNull(
            this.computerOffSource,
            "Unable to find off button audio source in Computer."
        );

        Assert.IsNotNull(this.fanLoop, "Fan loop is null.");

        fanStart.volume = fanVolume;
        fanLoop.volume = fanVolume;
        fanStop.volume = fanVolume;

        fanLoop.loop = true;

        switch (computer_.zone)
        {
            case Globals.Zone.A:
                this.computerZoneAudioSource = zoneALoop;
                break;
            case Globals.Zone.B:
                this.computerZoneAudioSource = zoneBLoop;
                break;
            case Globals.Zone.C:
                this.computerZoneAudioSource = zoneCLoop;
                break;
            case Globals.Zone.Broken:
                this.computerZoneAudioSource = brokenLoop;
                break;
            default:
                Debug.LogError(
                    $"Invalid computer zone in computer. Zone: {computer_.zone.GetType()}."
                );
                return;
        }

        Assert.IsNotNull(
            computerZoneAudioSource,
            "Unable to find zone audio source from computer."
        );

        originalVolume = computerZoneAudioSource.volume;

        computerZoneAudioSource.maxDistance = 5;
        computerZoneAudioSource.volume = 0;
        computerZoneAudioSource.Play();
    }

    public override void ToggleOn()
    {
        if (base.SoundEnabled())
            return;

        try
        {
            computerOnSource.Play();
            fanStart.Play();

            // var currentTime = AudioSettings.dspTime;
            fanLoop.PlayScheduled(fanStart.clip.length);

            computerZoneAudioSource.volume = originalVolume * zoneVolume;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Unable to play computer on sound. Exception: {e}");
        }
    }

    public override void ToggleOff()
    {
        if (!base.SoundEnabled())
            return;

        try
        {
            computerZoneAudioSource.volume = 0;
            computerOffSource.Play();
            fanLoop.Stop();
            fanStop.Play();
        }
        catch
        {
            Debug.LogError("Unable to play computer off sound.");
        }
    }
}
