using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayMerchant : DisplayInventory
{
    [SerializeField] private Text _buyText;
    [SerializeField] private InventoryObject _playerInventory;

    private ConsumableObject _consumable;
    private Player _player;
    private UIHUDManager _hudManager;
    //private UICurrentCoins _currentCoin;

    private void Awake()
    {
        _player = FindObjectOfType(typeof(Player)) as Player;
        //_hudManager = FindObjectOfType(typeof(UIHUDManager)) as UIHUDManager;


        for (int i = 0; i < _inventory._inventoryContainer.Count; i++)
        {
            var obj = Instantiate(_inventory._inventoryContainer[i]._consumable._prefabShop, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<Text>().text = _inventory._inventoryContainer[i]._amount.ToString("n0");
        }
    }

    private void Update()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        for (int i = 0; i < _inventory._inventoryContainer.Count; i++)
        {
            if (_consumableDisplayed.ContainsKey(_inventory._inventoryContainer[i]))
            {
                //_consumableDisplayed[_inventory._inventoryContainer[i]].GetComponentInChildren<Text>().text = _inventory._inventoryContainer[i]._amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(_inventory._inventoryContainer[i]._consumable._prefabShop, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                _consumableDisplayed.Add(_inventory._inventoryContainer[i], obj);
            }
            UpdateAmount(i);
        }
    }

    private void UpdateAmount(int i)
    {
        if(i < _playerInventory._inventoryContainer.Count)
        {
            for(int j = 0; j < _playerInventory._inventoryContainer.Count; j++)
            if (_inventory._inventoryContainer[i]._consumable == _playerInventory._inventoryContainer[j]._consumable)
            {
                _consumableDisplayed[_inventory._inventoryContainer[i]].GetComponentInChildren<Text>().text = _playerInventory._inventoryContainer[j]._amount.ToString("n0");
            }
        }
        else
        _consumableDisplayed[_inventory._inventoryContainer[i]].GetComponentInChildren<Text>().text = "0";

    }

    public void SaveBuyRef(ConsumableObject consumable)
    {
        _consumable = consumable;
    }

    public void BuyDisplay()
    {
        _player = FindObjectOfType(typeof(Player)) as Player;

        if (_player.NbOfCoins >= _consumable._price)
        {
            _playerInventory.AddConsumable(_consumable, 1);
            _player.NbOfCoins -= _consumable._price;
            EventManager.TriggerEvent(Events.PLAYER_COIN_CHANGE, new Dictionary<string, object> { { "coin", _player.NbOfCoins } });
        }
    }
}
