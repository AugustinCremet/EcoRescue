using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ConsumableType
{
    POTION,
    STAMINA,
    BOMB,
    POWER,
    DEFAULT
}
public abstract class ConsumableObject : ScriptableObject, ISellable
{
    protected Player _player;
    protected GameObject _playerGO;
    
    public GameObject _prefabInventory;
    public GameObject _prefabQuickUse;
    public GameObject _prefabShop;
    public ConsumableType _type;
    public Sprite _icon;
    public int _price;
    public string _name;
    [TextArea(15, 20)]
    public string _description;

    public virtual bool UseConsumable() { return false; }

    public string Name
    {
        get { return _name;}
    }
    public string Description
    {
        get { return _description; }
    }
    public int Price
    {
        get { return _price; }
    }
}
