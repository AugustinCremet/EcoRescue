using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HUDDescription
{
    [SerializeField] private GameObject _sectionCover;
    [SerializeField] private string _description;

    public void ActivateCover()
    {
        _sectionCover.SetActive(true);
    }

    public void DeactivateCover()
    {
        _sectionCover.SetActive(false);
    }

    public string Description => _description;
}
