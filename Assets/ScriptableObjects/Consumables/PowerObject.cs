using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Power Potion Object", menuName = "Inventory System/Consumables/Power")]
public class PowerObject : ConsumableObject
{
    public float _increasedPercentageValue = 0.3f;
    public int _timeSeconds = 60;
    private bool _isInUse = false;

    private DungeonManager _dungeonPlayer;
    private UIDelay _coroutine;

    public void Awake()
    {
        _type = ConsumableType.POWER;
        _isInUse = false;
    }

    public override bool UseConsumable() 
    {
        if (_isInUse == false)
        {
            _coroutine = FindObjectOfType(typeof(UIDelay)) as UIDelay;
            _coroutine.StartCoroutine(EffectActivated());
            return true;
        }
        else return false;
    }

    public void ResetPowerPotion()
    {
        _isInUse = false;
    }

    private IEnumerator EffectActivated()
    {
        _dungeonPlayer = FindObjectOfType(typeof(DungeonManager)) as DungeonManager;
        _player = _dungeonPlayer.Player.GetComponent<Player>();

        //Store previous stats
        var previousLightAttackRatio = _player.LightAttackDamage;
        var previousHeavyAttackRatio = _player.HeavyAttackDamage;
        var previousChargeAttackRatio = _player.ChargeAttackDamage;
        var previousProjectileAttackRatio = _player.ProjectileDamage;

        //Temporary increased damage 
        _player.LightAttackDamage += (int)(_player.LightAttackDamage * _increasedPercentageValue);
        _player.HeavyAttackDamage += (int)(_player.HeavyAttackDamage * _increasedPercentageValue);
        _player.ChargeAttackDamage += (int)(_player.ChargeAttackDamage * _increasedPercentageValue);
        _player.ProjectileDamage += (int)(_player.ProjectileDamage * _increasedPercentageValue);

        _isInUse = true;
        _player.UsePower(true);
        yield return new WaitForSecondsRealtime(_timeSeconds);
        _isInUse = false;
        _player.UsePower(false);
        

        //Get previous stats
        _player.LightAttackDamage = previousLightAttackRatio;
        _player.HeavyAttackDamage = previousHeavyAttackRatio;
        _player.ChargeAttackDamage = previousChargeAttackRatio;
        _player.ProjectileDamage = previousProjectileAttackRatio;
    }
}
