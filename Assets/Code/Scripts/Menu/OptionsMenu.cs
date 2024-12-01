using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    private Slider soundSlider;

    [SerializeField]
    private Slider sensitivtySlider;

    [SerializeField]
    private AudioMixer masterMixer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 1));
        SetSensitivity(PlayerPrefs.GetFloat("SavedSensitivty", 0.5f));
    }

    private void SetSensitivity(float v)
    {
        if (v < .00001f)
        {
            v = .00001f;
        }
        RefreshSensitivtySlider(v);
        PlayerPrefs.SetFloat("SavedSensitivty", v);
    }

    private void SetVolume(float v)
    {
        if (v < .00001f)
        {
            v = .00001f;
        }
        RefreshVolumeSlider(v);
        PlayerPrefs.SetFloat("SavedMasterVolume", v);
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(v) * 20f);
    }

    public void SetVolumeFromSlider()
    {
        SetVolume(soundSlider.value);
    }

    public void SetSensitivtyFromSlider()
    {
        SetSensitivity(sensitivtySlider.value);
    }

    private void RefreshVolumeSlider(float v)
    {
        soundSlider.value = v;
    }

    private void RefreshSensitivtySlider(float v)
    {
        sensitivtySlider.value = v;
    }
}
