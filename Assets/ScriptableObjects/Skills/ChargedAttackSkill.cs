using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Charged Attack Skill", menuName = "Inventory System/Skills/ChargedAttack")]
public class ChargedAttackSkill : SkillObject
{
    public bool _quickStrike = false;
    public float _decreasedPercentageValue = 0.15f;
    private float _previousValue;

    public bool _radiusPocus = false;
    public float _increasedRadiusValue = 1.5f;
    private float _previousRadius;

    private DungeonManager _dungeonPlayer;

    public void Awake()
    {
        _type = SkillType.CHARGEDATTACK;
    }

    public override bool UnlockSkill()
    {
        _dungeonPlayer = FindObjectOfType(typeof(DungeonManager)) as DungeonManager;
        _player = _dungeonPlayer.Player.GetComponent<Player>();

        if (_quickStrike)
        {
            _previousValue = _player.ChargeAttackCooldown;

            _player.ChargeAttackCooldown -= (float)(_player.ChargeAttackCooldown * _decreasedPercentageValue);
            return true;
        }
        else if (_radiusPocus)
        {
            _previousRadius = _player.ChargeAttackRadius;

            _player.ChargeAttackRadius += _increasedRadiusValue;
            return true;
        }
        else return false;
    }

    public override void ResetSkill()
    {
        if (_quickStrike) _player.ChargeAttackCooldown = _previousValue;

        if (_radiusPocus) _player.ChargeAttackRadius = _previousRadius;
    }
}
