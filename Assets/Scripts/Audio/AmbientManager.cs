using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum AmbientLabel
{
    Bird,
    Chainsaw,
    Weather,
    Leaves,
    AmbientLabelEnd
}

public class AmbientManager : AudioManager
{
    private List<List<Ambient>> _labelAmbients = new();
    
    private List<Ambient> _tracks = new();
    
    private bool _hostilePlaying;
    public int HostileNumber { get; set; }

    private void Start()
    {
        float fl = PlayerPrefs.GetFloat("AmbientVolume", 0);
        AdjustVolume("AmbientVolume", fl);
    }

    void Awake()
    {
        for (int i = 0; i < (int)AmbientLabel.AmbientLabelEnd; i++)
        {
            _labelAmbients.Add(new());
        }
    }
    
    public void ResetManager(AudioSource[] audioSources)
    {
        List<List<Ambient>> newLabelAmbients = new List<List<Ambient>>();
        List<Ambient> newAmbients;
        
        foreach (List<Ambient> ambients in _labelAmbients)
        {
            newAmbients = new List<Ambient>();
            
            foreach (Ambient ambient in ambients)
            {
                if(ambient._source != null)
                {
                    ambient._source.Stop();
                    if (!audioSources.Contains(ambient._source))
                        newAmbients.Add(ambient);
                }
            }
            
            newLabelAmbients.Add(newAmbients);
        }

        _labelAmbients = newLabelAmbients;
        
        newAmbients = new List<Ambient>();
            
        foreach (Ambient ambient in _tracks)
        {
            if (ambient._source != null)
            {
                ambient._source.Stop();
                if (!audioSources.Contains(ambient._source))
                    newAmbients.Add(ambient);
            }
        }
            
        _tracks = newAmbients;
        
        StopAllCoroutines();
    }
    
    public void Play(Ambient ambient)
    {
        if (ambient == null || ambient._source == null)
            return;
        ambient._source.Play();
    }

    private void Stop(int track = -1)
    {
        for (int i = 0; i < _tracks.Count; i++)
        {
            Ambient a = _tracks[i];
            if (a != null)
            {
                if (a._source.isPlaying && (track == i || track == -1))
                {
                    a._source.Stop();
                }
            }
        }
    }

    private void Pause(int track = -1)
    {
        for (int i = 0; i < _tracks.Count; i++)
        {
            Ambient a = _tracks[i];
            if (a != null)
            {
                if (a._source.isPlaying && (track == i || track == -1))
                {
                    a._source.Pause();
                }
            }
        }
    }

    public void AddAmbient(AudioClip a, AmbientAddressables ambient, AmbientLabel aLabel = AmbientLabel.AmbientLabelEnd, GameObject attachedTo = null)
    {
        attachedTo = attachedTo == null || attachedTo.name == "Addressables" ? gameObject : attachedTo;
        Ambient newAmbient = new()
        {
            _name = ambient._name == null ? a.ToString() : ambient._name,
            _clip = a,
            _pitch = ambient._pitch,
            _volume = ambient._volume,
            _loop = ambient._loop,
            _spatialBlend = ambient._spatialBlend,
            _minDistance = ambient._minDistance,
            _maxDistance = ambient._maxDistance,
            _audioRolloff = ambient._audioRolloff,
            _playOnAwake = ambient._playOnAwake,
            _source = attachedTo.AddComponent<AudioSource>()
        };
        newAmbient._source.clip = newAmbient._clip;
        newAmbient._source.volume = newAmbient._volume;
        newAmbient._source.pitch = newAmbient._pitch;
        newAmbient._source.loop = newAmbient._loop;
        newAmbient._source.spatialBlend = newAmbient._spatialBlend;
        newAmbient._source.minDistance = newAmbient._minDistance;
        newAmbient._source.maxDistance = newAmbient._maxDistance;
        newAmbient._source.rolloffMode = newAmbient._audioRolloff;
        newAmbient._source.playOnAwake = newAmbient._playOnAwake;

        newAmbient._source.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("Ambient")[0];
        
        if (aLabel != AmbientLabel.AmbientLabelEnd)
            _labelAmbients[(int)aLabel].Add(newAmbient);
        else
            _tracks.Add(newAmbient);

        if(newAmbient._playOnAwake)
            Play(newAmbient);
    }
    
    public void PlayAmbient(string s)
    {
        string n;
        
        foreach(Ambient ambient in _labelAmbients[(int)AmbientLabel.Bird])
        {
            n = ambient._name;
            
            if(n.IndexOf(" (") != -1)
            {
                n = n.Remove(n.IndexOf(" ("));
            }

            if(ambient._name.IndexOf(s) != -1 || n == s)
            {
                Play(ambient);
                return;
            }
        }
        
        foreach(Ambient ambient in _labelAmbients[(int)AmbientLabel.Chainsaw])
        {
            if(ambient._name.IndexOf(s) != -1)
            {
                Play(ambient);
                return;
            }
        }

        foreach (Ambient ambient in _tracks)
        {
            n = ambient._name;
            
            if(n.IndexOf(" (") != -1)
            {
                n = n.Remove(n.IndexOf(" ("));
            }
            
            Debug.Log(n);

            Debug.Log(s);

            if (ambient._name.IndexOf(s) != -1 || n == s)
            {
                Debug.Log(ambient._name);
                Play(ambient);
                return;
            }
        }
    }
}
