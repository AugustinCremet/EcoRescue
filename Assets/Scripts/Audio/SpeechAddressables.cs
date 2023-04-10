using UnityEngine;

[System.Serializable]
public class SpeechAddressables
{
    public string _path;
    public string _name;
    [Range(0f,1f)]
    public float _volume;
    public float _randomRepeat;
    [Range(0f, 1f)] 
    public float _spatialBlend;
    [Range(0f, 5f)] 
    public float _pitchMin;
    [Range(0f, 5f)] 
    public float _pitchMax;
}