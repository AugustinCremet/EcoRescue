using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SkillType
{
    ATTACK,
    CHARGEDATTACK,
    PROJECTILE,
    MOVEMENT,
    HEALTH,
    STAMINA
}
public abstract class SkillObject : ScriptableObject, ISellable
{
    protected Player _player;
    protected GameObject _playerGO;
    
    public GameObject _prefabSkillTree;
    public GameObject _prefabShop;
    public SkillType _type;
    public Sprite _icon;
    public int _price;
    public string _name;
    [TextArea(15, 20)]
    public string _description;

    public virtual bool UnlockSkill() { return false; }
    public virtual void ResetSkill() { }

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
