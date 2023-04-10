using UnityEngine;

[System.Serializable]
public class AmbientAddressables
{
    public string _path;
    public string _name;
    [Range(0f,1f)]
    public float _volume;
    [Range(0f,5f)]
    public float _pitch;
    public bool _loop;
    public float _spatialBlend;
    public float _minDistance = 10;
    public float _maxDistance = 30;
    public AudioRolloffMode _audioRolloff = AudioRolloffMode.Linear;
    public bool _playOnAwake;
}