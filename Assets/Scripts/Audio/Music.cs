using UnityEngine;

[System.Serializable]
public class Music
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
    public bool _persistent;
    
    public Music()
    {
        _volume = 1f;
    }
}