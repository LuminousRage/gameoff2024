using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    private Slider soundSlider;

    [SerializeField]
    private Slider musicSlider;

    [SerializeField]
    private Slider sensitivitySlider;

    [SerializeField]
    private Slider exposureSlider;

    [SerializeField]
    private AudioMixer soundEffectsMixer;

    [SerializeField]
    private AudioMixer musicMixer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnStart();
    }

    public void OnStart() {

        SetSoundEffectsVolume(PlayerPrefs.GetFloat("SavedSoundEffectsVolume", 1));
        SetMusicVolume(PlayerPrefs.GetFloat("SavedMusicVolume", 0.4f));
        SetSensitivity(PlayerPrefs.GetFloat("SavedSensitivity", 0.5f));
        SetSensitivity(PlayerPrefs.GetFloat("Exposure", 0.5f));
    }

    private void SetSensitivity(float v)
    {
        if (v < .00001f)
        {
            v = .00001f;
        }
        RefreshsensitivitySlider(v);
        PlayerPrefs.SetFloat("SavedSensitivity", v);
        PlayerPrefs.Save();
    }

    private void SetExposure(float v)
    {
        if (v < .00001f)
        {
            v = .00001f;
        }
        RefreshExposureSlider(v);
        PlayerPrefs.SetFloat("Exposure", v);
        PlayerPrefs.Save();
    }

    private void SetSoundEffectsVolume(float v)
    {
        if (v < .00001f)
        {
            v = .00001f;
        }
        RefreshSoundEffectsSlider(v);
        PlayerPrefs.SetFloat("SavedSoundEffectsVolume", v);
        PlayerPrefs.Save();

        soundEffectsMixer.SetFloat("MasterVolume", Mathf.Log10(v) * 20f);
    }

    private void SetMusicVolume(float v)
    {
        if (v < .00001f)
        {
            v = .00001f;
        }
        RefreshMusicSlider(v);
        PlayerPrefs.SetFloat("SavedMusicVolume", v);
        PlayerPrefs.Save();

        musicMixer.SetFloat("MasterVolume", Mathf.Log10(v) * 20f);
    }

    public void SetSoundVolumeFromSlider()
    {
        SetSoundEffectsVolume(soundSlider.value);
    }

    public void SetMusicVolumeFromSlider()
    {
        SetMusicVolume(musicSlider.value);
    }

    public void SetsensitivityFromSlider()
    {
        SetSensitivity(sensitivitySlider.value);
    }

    public void SetExposureFromSlider()
    {
        SetExposure(exposureSlider.value);
    }

    private void RefreshMusicSlider(float v)
    {
        musicSlider.value = v;
    }

    private void RefreshSoundEffectsSlider(float v)
    {
        soundSlider.value = v;
    }

    private void RefreshsensitivitySlider(float v)
    {
        sensitivitySlider.value = v;
    }

    private void RefreshExposureSlider(float v)
    {
        exposureSlider.value = v;
    }
}
