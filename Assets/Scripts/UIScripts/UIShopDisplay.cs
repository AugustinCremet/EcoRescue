using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopDisplay : MonoBehaviour
{
    [SerializeField] private ConsumableObject _consumable;

    [SerializeField] private Text _titleText;
    [SerializeField] private Text _descriptionText;
    [SerializeField] private Text _coinText;

    private void Awake()
    {
        _titleText.text = _consumable._name;
        _descriptionText.text = _consumable._description;
        _coinText.text = _consumable._price.ToString("n0");
    }
}
