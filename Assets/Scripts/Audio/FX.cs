using UnityEngine;

[System.Serializable]
public class FX
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
    [HideInInspector]
    public GameObject _parent;
    public float _randomRepeat;
    [HideInInspector]
    public float _pitchMin;
    [HideInInspector]
    public float _pitchMax;

    public FX()
    {
        _volume = 1f;
        _pitch = 1f;
        _pitchMin = 1f;
        _pitchMax = 1f;
    }
}
