using UnityEngine;

public class UIAudioSoundController : MonoBehaviour
{
    private float _cooldown;
    private bool _coolingDown;
    [SerializeField] private string _volumeType;

    void Start()
    {
    }

    public void TriggerVolumeSound()
    {
        if(_cooldown < Time.time && _cooldown != 0)
        {
            switch (_volumeType)
            {
                case "mastervolume":
                    FindObjectOfType<SpeechManager>().PlaySpeech("speechvolume");
                    FindObjectOfType<FXManager>().PlaySound("fxvolume", FindObjectOfType<AudioManager>().gameObject);
                    FindObjectOfType<AmbientManager>().PlayAmbient("ambientvolume");
                    break;
                case "musicvolume":
                    break;
                case "fxvolume":
                    FindObjectOfType<FXManager>().PlaySound(_volumeType, FindObjectOfType<AudioManager>().gameObject);
                    break;
                case "ambientvolume":
                    FindObjectOfType<AmbientManager>().PlayAmbient(_volumeType);
                    break;
                case "speechvolume":
                    FindObjectOfType<SpeechManager>().PlaySpeech(_volumeType);
                    break;
            }
            _cooldown = Time.time + 1f;
        } 
        else if (_cooldown == 0)
        {
            _cooldown = Time.time + 1f;
        }
    }
}
