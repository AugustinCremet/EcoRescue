using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillDisplay : MonoBehaviour
{
    [SerializeField] private SkillObject _skill;

    [SerializeField] private Text _titleText;
    [SerializeField] private Text _descriptionText;
    [SerializeField] private Text _coinText;
    [SerializeField] private bool _containText;

    private void Awake()
    {
        _titleText.text = _skill._name;
        _descriptionText.text = _skill._description;
        if (_containText) _coinText.text = _skill._price.ToString("n0");
    }
}
