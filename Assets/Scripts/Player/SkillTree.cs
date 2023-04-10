using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    private Player _player;
    [SerializeField] private int _lightAttackInc = 10;
    [SerializeField] private int _heavyAttackInc = 20;
    [SerializeField] private float _speedInc = 1f;
    [SerializeField] private int _healthInc = 1;
    [SerializeField] private int _staminaInc = 1;
    [SerializeField] private int _chargeDamageInc = 50;
    [SerializeField] private float _chargeCooldownInc = 1f;
    [SerializeField] private float _chargeRadiusInc = 1f;
    [SerializeField] private int _projectileDamageInc = 10;
    [SerializeField] private float _projectileCooldownInc = 2f;
    //[SerializeField] private float _projectileDistanceInc = 2f;
    [SerializeField] private int _dashInc = 1;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    public void BasicAttack()
    {
        _player.LightAttackDamage += _lightAttackInc;
        _player.HeavyAttackDamage += _heavyAttackInc;
    }

    public void Speed()
    {
        _player.Speed += _speedInc;
    }

    public void MaxHealth()
    {
        _player.MaxHealth += _healthInc;
    }

    public void MaxStamina()
    {
        _player.MaxStamina += _staminaInc;
    }

    public void ChargeAttackDamage()
    {
        _player.ChargeAttackDamage += _chargeDamageInc;
    }

    public void ChargeAttackCooldown()
    {
        _player.ChargeAttackCooldown -= _chargeCooldownInc;
    }

    public void ChargeAttackRadius()
    {
        _player.ChargeAttackRadius += _chargeRadiusInc;
    }

    public void ProjectileDamage()
    {
        _player.ProjectileDamage += _projectileDamageInc;
    }

    public void ProjectileCooldown()
    {
        _player.ProjectileCooldown -= _projectileCooldownInc;
    }

    public void ProjectileDistance()
    {
        _player.ProjectileDistance += _projectileDamageInc;
    }

    public void DashCound()
    {
        _player.DashCount += _dashInc;
    }
}
