using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimState : SkillBaseState
{
    public AimState(SkillSM sm, Animator animator, Player player, FXManager fxManager, SpeechManager speechManager) : base (sm ,player, animator, fxManager, speechManager)
    { 
        _sm = sm;
        _animator = animator;
    }

    public override void Enter()
    {
        base.Enter();
        _animator.ResetTrigger("passive");
        _animator.SetBool("isCharging", false);
        _animator.SetBool("isAiming", true);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (_isUsingKeyboard)
            _sm.transform.LookAt(new Vector3(_sm.MousePositionObject.transform.position.x, _sm.transform.position.y, _sm.MousePositionObject.transform.position.z));

        if (!_sm.IsAiming)
        {
            _animator.SetTrigger("passive");
            _sm.ChangeState(_sm.Passive);
        }
        if(_sm.IsShooting && _player.IsProjectileReady && _player.CurrentStamina >= 1)
        {
            _sm.ChangeState(_sm.Shoot);
        }
        else if(_sm.IsShooting && _player.IsProjectileReady && _player.CurrentStamina < 1)
        {
            EventManager.TriggerEvent(Events.PLAYER_STAMINA_CHANGE,
                    new Dictionary<string, object> { { "staminaMissing", 1 } });
        }

        if(_sm.IsCharging && _player.IsChargeReady && _player.CurrentStamina >= 2)
        {
            _sm.ChangeState(_sm.Charging);
            
        }
        else if(_sm.IsCharging && _player.IsChargeReady && _player.CurrentStamina < 2)
        {
            EventManager.TriggerEvent(Events.PLAYER_STAMINA_CHANGE,
                    new Dictionary<string, object> { { "staminaMissing", 2 } });
        }

    }

    public override void Exit()
    {
        base.Exit();
        _animator.SetBool("isAiming", false);
    }
}
