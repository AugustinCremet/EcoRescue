using UnityEngine;

[System.Serializable]
public class Speech
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
    public float _spatialBlend;
    public float _randomRepeat;
    [HideInInspector]
    public float _pitchMin;
    [HideInInspector]
    public float _pitchMax;
    [HideInInspector] 
    public bool _persistent;

    public Speech()
    {
        _volume = 1f;
        _pitch = 1f;
        _pitchMin = 1f;
        _pitchMax = 1f;
    }
}