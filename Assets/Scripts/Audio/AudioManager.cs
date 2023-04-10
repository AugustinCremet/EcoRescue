using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;

    public AudioMixer _audioMixer;

    private float _musicVolume;

    private void Start()
    {
        float fl = PlayerPrefs.GetFloat("MasterVolume", 0);
        AdjustVolume("MasterVolume", fl);
    }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        
        EventManager.StartListening(Events.SWITCH_ROOM, ResetManager);
    }

    private void ResetManager(Dictionary<string, object> message)
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        
        GetComponent<MusicManager>().ResetManager(audioSources);
        GetComponent<FXManager>().ResetManager(audioSources);
        GetComponent<AmbientManager>().ResetManager(audioSources);
        GetComponent<SpeechManager>().ResetManager(audioSources);
    }

    private void OnMainMixerLoaded(AsyncOperationHandle<AudioMixer> handle)
    {
        _audioMixer = handle.Result;
    }

    protected void AdjustVolume(string volumeType, float newVolume)
    {
        newVolume = (1 - Mathf.Sqrt(newVolume)) * -80f;
            _audioMixer.SetFloat(volumeType, newVolume);
    }
}
