using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public enum FXLabel
{
    Player,
    Enemy,
    Bird,
    BossLog,
    FXLabelEnd
}

public class FXManager : AudioManager
{
    private List<List<FX>> _labelSounds = new List<List<FX>>();

    private List<FX> _tracks = new List<FX>();
    
    private void Start()
    {
        float fl = PlayerPrefs.GetFloat("FXVolume", 0);
        AdjustVolume("FXVolume", fl);
    }

    private void Awake()
    {
        for (int i = 0; i < (int)FXLabel.FXLabelEnd; i++)
        {
            _labelSounds.Add(new List<FX>());
        }
    }

    private void PlayFootsteps(Dictionary<string, object> message)
    {
        if ((int)message["walking"] > 0)
        {
            int randomFootsteps = Random.Range(1, 4);
            PlaySound("footsteps" + randomFootsteps, gameObject, true);
        }
    }
    
    public void ResetManager(AudioSource[] audioSources)
    {
        List<List<FX>> newLabelSounds = new List<List<FX>>();
        List<FX> newSounds;
        
        foreach (List<FX> sounds in _labelSounds)
        {
            newSounds = new List<FX>();
            
            foreach (FX sound in sounds)
            {
                if (!audioSources.Contains(sound._source))
                    newSounds.Add(sound);
            }
            
            newLabelSounds.Add(newSounds);
        }

        _labelSounds = newLabelSounds;
        
        newSounds = new List<FX>();
            
        foreach (FX sound in _tracks)
        {
            if (!audioSources.Contains(sound._source))
                newSounds.Add(sound);
        }
            
        _tracks = newSounds;
        
        StopAllCoroutines();
    }

    private void StopFootsteps(Dictionary<string, object> message)
    {
        if ((int)message["idle"] > 0)
        {
            StopSound("footsteps1", gameObject);
            StopSound("footsteps2", gameObject);
            StopSound("footsteps3", gameObject);
        }
    }
    
    public void Play(FX sound)
    {
        if (sound == null || sound._source == null) return;
        
        if (sound._pitchMin != 1 || sound._pitchMax != 1)
            sound._source.pitch = Random.Range(sound._pitchMin, sound._pitchMax);

        if (sound._randomRepeat != 0)
        {
            StartCoroutine(PlayRepeatable(sound));
            if (Random.Range(0, 2) == 0)
                sound?._source.Play();
        }
        else if(!sound._source.isPlaying)
            sound?._source.Play();
    }
    
    private IEnumerator PlayRepeatable(FX sound)
    {
        yield return new WaitForSeconds (sound._randomRepeat);
        Play(sound);
    }

    public void AddFX(AudioClip a, FXAddressables sound, FXLabel fxLabel = FXLabel.FXLabelEnd, GameObject attachedTo = null)
    {
        attachedTo = attachedTo == null ? attachedTo.name == "Addressables" ? FindObjectOfType<AudioManager>().gameObject : gameObject : attachedTo;
        FX newSound = new()
        {
            _name = sound._name == null ? a.ToString() : sound._name,
            _clip = a,
            _pitch = 1f,
            _volume = sound._volume,
            _randomRepeat = sound._randomRepeat,
            _parent = attachedTo,
            _spatialBlend = attachedTo.name == "AudioManager" || attachedTo.name == "Addressables"? 0 : 1,
            _pitchMin = sound._pitchMin,
            _pitchMax = sound._pitchMax,
            _source = attachedTo.AddComponent<AudioSource>()
        };
        newSound._source.clip = newSound._clip;
        newSound._source.volume = newSound._volume;
        newSound._source.pitch = newSound._pitch;
        newSound._source.spatialBlend = newSound._spatialBlend;
        newSound._source.rolloffMode = AudioRolloffMode.Linear;
        newSound._source.maxDistance = 20f;
        
        newSound._source.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("FX")[0];
        
        if (fxLabel != FXLabel.FXLabelEnd)
            _labelSounds[(int)fxLabel].Add(newSound);
        else
            _tracks.Add(newSound);
                
        if (newSound._randomRepeat != 0)
            StartCoroutine(PlayRepeatable(newSound));

    }

    public void PlaySound(string s, GameObject parent, bool loop = false, float pitchVariation = 0f)
    {
        for (int i = 0; i < (int)FXLabel.FXLabelEnd; i++)
        {
            foreach (FX sound in _labelSounds[i])
            {
                if (sound._name.IndexOf(s) != -1 && sound._parent == parent)
                {
                    sound._source.loop = loop;
                    sound._pitchMin = pitchVariation != 0 ? 1 - sound._pitchMin * pitchVariation : sound._pitchMin;
                    sound._pitchMax = pitchVariation != 0 ? 1 + sound._pitchMax * pitchVariation : sound._pitchMax;
                    Play(sound);
                    return;
                }
            }
        }
        
        foreach (FX sound in _tracks)
        {
            if (sound._name.IndexOf(s) != -1 && sound._parent == parent)
            {
                sound._source.loop = loop;
                sound._pitchMin = pitchVariation != 0 ? sound._pitchMin - sound._pitchMin * pitchVariation : sound._pitchMin;
                sound._pitchMax = pitchVariation != 0 ? sound._pitchMax + sound._pitchMax * pitchVariation : sound._pitchMax;
                if(!sound._source.isPlaying)
                    Play(sound);
                return;
            }
        }
    }
    
    public void StopSound(string s, GameObject parent)
    {
        foreach(FX sound in _labelSounds[(int)FXLabel.Player])
        {
            if(sound._name.IndexOf(s) != -1 && sound._parent == parent)
            {
                sound._source.Stop();
                return;
            }
        }

        foreach (FX sound in _tracks)
        {
            if (sound._name.IndexOf(s) != -1 && sound._parent == parent)
            {
                sound._source.Stop();
                return;
            }
        }
    }
    
    public void StopAllSound(Dictionary<string, object> message)
    {
        foreach(FX sound in _labelSounds[(int)FXLabel.Player])
        {
            if (sound != null)
                if(sound._source != null)
                    sound._source.Stop();
        }
        
        foreach(FX sound in _labelSounds[(int)FXLabel.Enemy])
        {
            if (sound != null)
                if(sound._source != null)
                    sound._source.Stop();
        }
        
        foreach(FX sound in _labelSounds[(int)FXLabel.Bird])
        {
            if (sound != null)
                if(sound._source != null)
                    sound._source.Stop();
        }
        
        foreach(FX sound in _labelSounds[(int)FXLabel.BossLog])
        {
            if (sound != null)
                if(sound._source != null)
                    sound._source.Stop();
        }

        foreach (FX sound in _tracks)
        {
            if (sound != null)
                if(sound._source != null)
                    sound._source.Stop();
        }
    }
    
    private void OnEnable()
    {
        EventManager.StartListening(Events.PLAYER_WALKING, PlayFootsteps);
        EventManager.StartListening(Events.PLAYER_IDLE, StopFootsteps);
        EventManager.StartListening(Events.PAUSE, StopAllSound);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.PLAYER_WALKING, PlayFootsteps);
        EventManager.StopListening(Events.PLAYER_IDLE, StopFootsteps);
        EventManager.StopListening(Events.PAUSE, StopAllSound);
    }
    
}
