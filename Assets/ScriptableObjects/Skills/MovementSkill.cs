using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Movement Skill", menuName = "Inventory System/Skills/Movement")]
public class MovementSkill : SkillObject
{
    public float _increasedPercentageValue = 0.1f;
    private DungeonManager _dungeonPlayer;

    private float _previousSpeed;

    public void Awake()
    {
        _type = SkillType.MOVEMENT;
    }

    public override bool UnlockSkill()
    {
        _dungeonPlayer = FindObjectOfType(typeof(DungeonManager)) as DungeonManager;
        _player = _dungeonPlayer.Player.GetComponent<Player>();

        _previousSpeed = _player.Speed;

        _player.Speed += (int)(_player.Speed * _increasedPercentageValue);
        return true;
    }

    public override void ResetSkill()
    {
        _player.Speed = _previousSpeed;
    }
}
