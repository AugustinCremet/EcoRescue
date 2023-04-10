using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stamina Skill", menuName = "Inventory System/Skills/Stamina")]
public class StaminaSkill : SkillObject
{
    public bool _eatYourVeggies = false;
    public int _increasedValue = 1;

    public bool _springSpring = false;

    public bool _springSpringSpring = false;

    private DungeonManager _dungeonPlayer;

    private int _previousStats;

    public void Awake()
    {
        _type = SkillType.STAMINA;
    }

    public override bool UnlockSkill()
    {
        _dungeonPlayer = FindObjectOfType(typeof(DungeonManager)) as DungeonManager;
        _player = _dungeonPlayer.Player.GetComponent<Player>();

        if (_eatYourVeggies)
        {
            _previousStats = _player.MaxStamina;

            _player.MaxStamina += _increasedValue;
            return true;
        }
        else if (_springSpring)
        {
            _player.DashCount = 2;
            return true;
        }
        else if (_springSpringSpring)
        {
            _player.DashCount = 3;
            return true;
        }
        return false;
    }

    public override void ResetSkill()
    {
        if (_eatYourVeggies)
        {
            _player.MaxStamina = _previousStats;
        } 
    }
}
