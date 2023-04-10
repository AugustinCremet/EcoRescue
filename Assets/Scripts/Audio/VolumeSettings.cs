using UnityEngine;

using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string volumeMixerParam;
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = 1f;
        slider.minValue = 0f;
        slider.onValueChanged.AddListener(AdjustVolume);
    }

    void Start()
    {
        GetSavedValue();
    }

    void GetSavedValue()
    {
        slider.value = PlayerPrefs.GetFloat(volumeMixerParam, 0);
    }

    private void AdjustVolume(float volume)
    {
        float adjustedVolume;
        adjustedVolume = (1 - Mathf.Sqrt(volume)) * -80f;
        
        mixer.SetFloat(volumeMixerParam, adjustedVolume);
        PlayerPrefs.SetFloat(volumeMixerParam, volume);
    }
}
