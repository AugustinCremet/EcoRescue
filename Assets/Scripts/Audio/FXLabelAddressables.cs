using UnityEngine;

[System.Serializable]
public class FXLabelAddressables
{
    public FXLabel _label;
    [Range(0f,1f)]
    public float _volume;
    [Tooltip("AudioSource will not be destroyed when loading a new scene")]
    public bool _persistent;
}