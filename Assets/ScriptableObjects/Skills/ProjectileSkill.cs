using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Skill", menuName = "Inventory System/Skills/Projectile")]
public class ProjectileSkill : SkillObject
{
    public bool _rocketLauncher = false;
    public float _decreasedPercentageValue = 0.015f;
    private float _previousCooldown;

    public bool _extraLength = false;
    public float _increasedPercentageValue = 0.07f;
    private float _previousDistance;

    private DungeonManager _dungeonPlayer;

    public void Awake()
    {
        _type = SkillType.CHARGEDATTACK;
    }

    public override bool UnlockSkill()
    {
        _dungeonPlayer = FindObjectOfType(typeof(DungeonManager)) as DungeonManager;
        _player = _dungeonPlayer.Player.GetComponent<Player>();

        if (_rocketLauncher)
        {
            _previousCooldown = _player.ProjectileCooldown;

            _player.ProjectileCooldown -= (float)(_player.ProjectileCooldown * _decreasedPercentageValue);
            return true;
        }
        else if (_extraLength)
        {
            _previousDistance = _player.ProjectileDistance;

            _player.ProjectileDistance += (float)(_player.ProjectileDistance * _increasedPercentageValue);
            return true;
        }
        else return false;
    }

    public override void ResetSkill()
    {
        if (_rocketLauncher) _player.ProjectileCooldown = _previousCooldown;

        if (_extraLength) _player.ProjectileDistance = _previousDistance;
    }
}
