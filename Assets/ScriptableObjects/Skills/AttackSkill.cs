using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Skill", menuName = "Inventory System/Skills/Attack")]
public class AttackSkill : SkillObject
{
    public float _increasedPercentageValue = 0.1f;
    private DungeonManager _dungeonPlayer;

    private int _previousLight;
    private int _previousHeavy;

    public void Awake()
    {
        _type = SkillType.ATTACK;
    }

    public override bool UnlockSkill()
    {
        _dungeonPlayer = FindObjectOfType(typeof(DungeonManager)) as DungeonManager;
        _player = _dungeonPlayer.Player.GetComponent<Player>();

        _previousLight = _player.LightAttackDamage;
        _previousHeavy = _player.HeavyAttackDamage;
                        
        _player.LightAttackDamage += (int)(_player.LightAttackDamage * _increasedPercentageValue);
        _player.HeavyAttackDamage += (int)(_player.HeavyAttackDamage * _increasedPercentageValue);
        return true;
    }

    public override void ResetSkill()
    {
        _player.LightAttackDamage = _previousLight;
        _player.HeavyAttackDamage = _previousHeavy;
    }
}
