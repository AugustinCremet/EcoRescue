using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Health Skill", menuName = "Inventory System/Skills/Health")]
public class HealthSkill : SkillObject
{
    public int _additionnalLifeValue = 1;
    private DungeonManager _dungeonPlayer;

    private int _previousStats;

    public void Awake()
    {
        _type = SkillType.HEALTH;
    }

    public override bool UnlockSkill()
    {
        _dungeonPlayer = FindObjectOfType(typeof(DungeonManager)) as DungeonManager;
        _player = _dungeonPlayer.Player.GetComponent<Player>();

        _previousStats = _player.MaxHealth;

        _player.MaxHealth += _additionnalLifeValue;
        _player.GainHealth(_additionnalLifeValue);

        return true;
    }

    public override void ResetSkill()
    {
        _player.MaxHealth = _previousStats;
    }

}
