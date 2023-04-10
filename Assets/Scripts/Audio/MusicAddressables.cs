using UnityEngine;

[System.Serializable]
public class MusicAddressables
{
    public string _path;
    public string _name;
    [Range(0f,1f)]
    public float _volume;

    public bool _persistent;
}