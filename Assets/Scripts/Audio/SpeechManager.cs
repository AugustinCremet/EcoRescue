using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum SpeechLabel
{
    Speech,
    UI,
    Player,
    SpeechLabelEnd
}

public class SpeechManager : AudioManager
{
    private List<List<Speech>> _labelSpeeches = new();

    private List<Speech> _tracks = new();

    private void Start()
    {
        float fl = PlayerPrefs.GetFloat("SpeechVolume", 0);
        AdjustVolume("SpeechVolume", fl);
    }
    
    private void Awake()
    {
        for (int i = 0; i < (int)SpeechLabel.SpeechLabelEnd; i++)
        {
            _labelSpeeches.Add(new List<Speech>());
        }
    }
    
    public void ResetManager(AudioSource[] audioSources)
    {
        List<List<Speech>> newLabelSpeeches = new List<List<Speech>>();
        List<Speech> newSpeeches;
        
        foreach (List<Speech> speeches in _labelSpeeches)
        {
            newSpeeches = new List<Speech>();
            
            foreach (Speech speech in speeches)
            {
                if (!audioSources.Contains(speech._source))
                {
                    if (speech._persistent)
                        newSpeeches.Add(speech);
                    else
                        Destroy(speech._source);
                }
            }
            
            newLabelSpeeches.Add(newSpeeches);
        }

        _labelSpeeches = newLabelSpeeches;
        
        newSpeeches = new List<Speech>();
            
        foreach (Speech speech in _tracks)
        {
            if (!audioSources.Contains(speech._source))
            {
                if (speech._persistent)
                    newSpeeches.Add(speech);
                else
                    Destroy(speech._source);
            }
        }
            
        _tracks = newSpeeches;
        
        StopAllCoroutines();
    }
    
    public void Play(Speech speech)
    {
        if (speech == null || speech._source == null) return;

        if (speech._pitchMin != 1 || speech._pitchMax != 1)
            speech._source.pitch = Random.Range(speech._pitchMin, speech._pitchMax);

        if (speech._randomRepeat != 0)
        {
            StartCoroutine(PlayRepeatable(speech));
            if (Random.Range(0, 2) == 0)
                speech?._source.Play();
        }
        else
            speech?._source.Play();
    }

    private IEnumerator PlayRepeatable(Speech speech)
    {
        yield return new WaitForSeconds (speech._randomRepeat);
        Play(speech);
    }

    public void AddSpeech(AudioClip a, SpeechAddressables speech, SpeechLabel sLabel = SpeechLabel.SpeechLabelEnd, GameObject attachedTo = null)
    {
        attachedTo = attachedTo == null ? gameObject : attachedTo;
        Speech newSpeech = new()
        {
            _name = speech._name == null ? a.ToString() : speech._name,
            _clip = a,
            _pitch = 1f,
            _volume = speech._volume,
            _randomRepeat = speech._randomRepeat,
            _spatialBlend = speech._spatialBlend,
            _pitchMin = speech._pitchMin,
            _pitchMax = speech._pitchMax,
            _source = attachedTo.AddComponent<AudioSource>()
        };
        newSpeech._source.clip = newSpeech._clip;
        newSpeech._source.volume = newSpeech._volume;
        newSpeech._source.pitch = newSpeech._pitch;
        newSpeech._source.spatialBlend = newSpeech._spatialBlend;
        newSpeech._source.maxDistance = 30f;
        newSpeech._source.playOnAwake = false;

        newSpeech._source.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("Speech")[0];

        if (sLabel != SpeechLabel.SpeechLabelEnd)
            _labelSpeeches[(int)sLabel].Add(newSpeech);
        else
            _tracks.Add(newSpeech);
                
        if (newSpeech._randomRepeat != 0)
            StartCoroutine(PlayRepeatable(newSpeech));
        
    }

    public void PlaySpeech(string s)
    {
        string n = "";
        foreach(Speech speech in _labelSpeeches[(int)SpeechLabel.Speech])
        {
            if(speech._name.IndexOf(" (UnityEngine.AudioClip)") != -1)
            {
                n = speech._name.Remove(speech._name.IndexOf(" (UnityEngine.AudioClip)"));
            }
            
            if(n == s)
            {
                Play(speech);
                
                return;
            }
        }

        foreach (Speech speech in _tracks)
        {
            if(speech._name.IndexOf(" (UnityEngine.AudioClip)") != -1)
            {
                n = speech._name.Remove(speech._name.IndexOf(" (UnityEngine.AudioClip)"));
            }
            
            if(n == s)
            {
                Play(speech);

                return;
            }
        }
    }
    
    public void StopSpeech(string s)
    {
        string n = "";
        foreach(Speech speech in _labelSpeeches[(int)SpeechLabel.Speech])
        {
            if(speech._name.IndexOf(" (UnityEngine.AudioClip)") != -1)
            {
                n = speech._name.Remove(speech._name.IndexOf(" (UnityEngine.AudioClip)"));
            }
            
            if(n == s)
            {
                speech._source.Stop();
                return;
            }
        }

        foreach (Speech speech in _tracks)
        {
            if(speech._name.IndexOf(" (UnityEngine.AudioClip)") != -1)
            {
                n = speech._name.Remove(speech._name.IndexOf(" (UnityEngine.AudioClip)"));
            }
            
            if(n == s)
            {
                speech._source.Stop();
                return;
            }
        }
    }

    public void StopAllConsumables()
    {
        string s = "consumable";
        
        for(int i = 0; i < (int)Consumables.ConsumableEnd; i++)
        {
            s = ((Consumables)i).ToString();
            
            s = s.IndexOf(" ") != -1 ? s.Remove(s.IndexOf(" ")) : s;
            s = s.ToLower() + "description";

            FindObjectOfType<SpeechManager>().StopSpeech(s);
        }
    }
    
    public void StopAllSkills()
    {
        string s = "skill";
        
        for(int i = 0; i < (int)Skills.SkillsEnd; i++)
        {
            s = ((Skills)i).ToString();
            
            s = s.IndexOf(" ") != -1 ? s.Remove(s.IndexOf(" ")) : s;
            s = s.ToLower() + "description";

            FindObjectOfType<SpeechManager>().StopSpeech(s);
        }
    }
    
    private enum Consumables
    {
        Bomb,
        SmallPotion,
        StaminaPotion,
        PowerPotion,
        BigPotion,
        ConsumableEnd
    }
    
    private enum Skills
    {
        GrandDamage,
        CatLives,
        EatYourVeggies,
        ExtraLength,
        QuickStrike,
        RadiusPocus,
        RocketLauncher,
        Skateboard,
        SpringSpring,
        SpringSpringSpring,
        SkillsEnd
    }
}
