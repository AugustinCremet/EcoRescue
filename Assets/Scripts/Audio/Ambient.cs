using UnityEngine;

[System.Serializable]
public class Ambient
{
    [HideInInspector]
    public AudioSource _source;
    [HideInInspector]
    public string _name;
    [HideInInspector]
    public AudioClip _clip;
    [HideInInspector]
    public float _volume;
    [HideInInspector]
    public float _pitch;
    [HideInInspector]
    public bool _loop;
    [HideInInspector]
    public float _spatialBlend;
    [HideInInspector]
    public float _minDistance;
    [HideInInspector]
    public float _maxDistance;
    [HideInInspector]
    public AudioRolloffMode _audioRolloff;
    [HideInInspector] 
    public bool _playOnAwake;
    
    
    public Ambient()
    {
        _volume = 1f;
        _pitch = 1f;
        _loop = true;
        _playOnAwake = true;
        _minDistance = 10;
        _maxDistance = 30;
        _audioRolloff = AudioRolloffMode.Linear;
    }
}