using System.Collections;
using UnityEngine;

public class UICreditsSoundController : MonoBehaviour
{
    private float _cooldown;
    private bool _coolingDown;
    [SerializeField] private string _volumeType;
    private SpeechManager _speechManager;

    private void OnEnable()
    {
        _speechManager = FindObjectOfType<SpeechManager>();
        StartCoroutine(CreditsSound());
    }

    private IEnumerator CreditsSound()
    {
        yield return new WaitForSeconds(1f);
        
        _speechManager.PlaySpeech("unity");

        yield return new WaitForSeconds(1f);
        
        _speechManager.PlaySpeech("collegeboisdeboulogne");

        yield return new WaitForSeconds(2f);
        
        _speechManager.PlaySpeech("programmers");
        
        yield return new WaitForSeconds(1.4f);
        
        _speechManager.PlaySpeech("juliebordagesphonetic");
        
        yield return new WaitForSeconds(1.4f);
        
        _speechManager.PlaySpeech("augustincremetphonetic");
        
        yield return new WaitForSeconds(1.4f);
        
        _speechManager.PlaySpeech("alexislacassephonetic");
        
        yield return new WaitForSeconds(1.4f);
        
        _speechManager.PlaySpeech("antoinenoelphonetic");
    }
    
    public void TriggerVolumeSound()
    {
        if(_cooldown < Time.time && _cooldown != 0)
        {
            _speechManager.PlaySpeech(_volumeType);
            _cooldown = Time.time + 1f;
        } 
        else if (_cooldown == 0)
        {
            _cooldown = Time.time + 1f;
        }
    }
}
