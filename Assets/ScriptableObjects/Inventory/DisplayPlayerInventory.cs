using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlayerInventory : DisplayInventory
{
    [SerializeField] private Image _consumableIcon;
    [SerializeField] private Text _consumableTitle;
    [SerializeField] private Text _consumableDescription;

    private void ResetGame(Dictionary<string, object> message)
    {
        bool needReset = (bool)message["reset"];

        if (needReset == false) return;

        for (int i = 0; i < _inventory._inventoryContainer.Count; i++)
        {
            _inventory._inventoryContainer[i]._amount = 0;
        }
    }

    public void Update()
    {
        for (int i = 0; i < _inventory._inventoryContainer.Count; i++)
        {
            if (_consumableDisplayed.ContainsKey(_inventory._inventoryContainer[i]))
            {
                _consumableDisplayed[_inventory._inventoryContainer[i]].GetComponentInChildren<Text>().text = _inventory._inventoryContainer[i]._amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(_inventory._inventoryContainer[i]._consumable._prefabInventory, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<Text>().text = _inventory._inventoryContainer[i]._amount.ToString("n0");
                _consumableDisplayed.Add(_inventory._inventoryContainer[i], obj);
            }
        }
    }
    public void DisplayOverview(ConsumableObject consumable)
    {
        _consumableIcon.sprite = consumable._icon;
        _consumableTitle.text = consumable._name.ToString();
        _consumableDescription.text = consumable._description.ToString();
        
    }

    private void OnEnable()
    {
        EventManager.StartListening(Events.RESET, ResetGame);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.RESET, ResetGame);
    }
}
