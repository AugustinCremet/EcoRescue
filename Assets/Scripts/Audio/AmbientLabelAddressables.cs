using UnityEngine;

[System.Serializable]
public class AmbientLabelAddressables
{
    public AmbientLabel _label;
    [Range(0f,1f)]
    public float _volume;
    public bool _loop = true;
    public bool _playOnAwake = true;
    
    [Tooltip("AudioSource will not be destroyed when loading a new scene")]
    public bool _persistent;
    
    public float _spatialBlend = 0;
    public float _minDistance = 10;
    public float _maxDistance = 30;
    public AudioRolloffMode _audioRolloff;
}