using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayQuickUse : DisplayInventory
{
    [SerializeField] private Text _equipText;
    [SerializeField] private InventoryObject _playerInventory;

    private ConsumableObject _consumable;

    private void Awake()
    {
        _equipText.text = "       EQUIP";
    }

    private void ResetGame(Dictionary<string, object> message)
    {
        bool needReset = (bool)message["reset"];

        if (needReset == false) return;

        _equipText.text = "       EQUIP";

        _inventory._inventoryContainer.Clear();

        _inventory._inventoryContainer.Add(new InventorySlot(null, 1));
        _inventory._inventoryContainer.Add(new InventorySlot(null, 1));
        _inventory._inventoryContainer.Add(new InventorySlot(null, 1));
        _inventory._inventoryContainer.Add(new InventorySlot(null, 1));
    }

    private void Update()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        for (int i = 0; i < _inventory._inventoryContainer.Count; i++)
        {
            if (_inventory._inventoryContainer[i]._consumable == null) continue;

            if (_consumableDisplayed.ContainsKey(_inventory._inventoryContainer[i]))
            {
                _consumableDisplayed[_inventory._inventoryContainer[i]].GetComponentInChildren<Text>().text = _inventory._inventoryContainer[i]._amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(_inventory._inventoryContainer[i]._consumable._prefabQuickUse, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<Text>().text = _inventory._inventoryContainer[i]._amount.ToString("n0");
                _consumableDisplayed.Add(_inventory._inventoryContainer[i], obj);
            }
            RemoveWhenEmpty(i);
        }
    }

    private void ChangeEquipText()
    {
        if (_playerInventory.GetAmount(_playerInventory, _consumable) == 0 && _inventory.GetAmount(_inventory, _consumable) == 0)
            _equipText.text = "       NO ITEM";
        else if (_playerInventory.GetAmount(_playerInventory, _consumable) == 0)
            _equipText.text = "       UNEQUIP";
        else
            _equipText.text = "       EQUIP";
    }

    private void RemoveWhenEmpty(int i)
    {
        if (_inventory._inventoryContainer[i]._amount == 0)
        {
            _consumableDisplayed.Remove(_inventory._inventoryContainer[i]);
            _inventory._inventoryContainer[i]._consumable = null;
            _inventory._inventoryContainer[i]._amount = 1;

            return;
        }
    }

    public void SaveEquipRef(ConsumableObject consumable)
    {
        _consumable = consumable;
        ChangeEquipText();
    }
    public void EquipDisplay()
    {
        if (_playerInventory.GetAmount(_playerInventory, _consumable) == 0)
        {
            _playerInventory.SwapConsumable(_inventory, _playerInventory, _consumable); //QuickUse to inventory
        }
        else
        {
            _playerInventory.SwapConsumable(_playerInventory, _inventory, _consumable); //inventory to QuickUse
            return;
        }
        ChangeEquipText();
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
