using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveState : SkillBaseState
{
    public PassiveState(SkillSM sm, Player player, Animator animator, FXManager fxManager, SpeechManager speechManager) : base(sm, player, animator, fxManager, speechManager) { }

    public override void Enter()
    {
        base.Enter();
        _animator.ResetTrigger("lightAttack");
        _animator.ResetTrigger("heavyAttack");
    }

    public override void UpdateLogic()
    {
        base .UpdateLogic();
        if (!WaitForSeconds())
        {
            return;
        }

        if (_sm.IsAiming)
        {
            _sm.ChangeState(_sm.Aim);
        }
        else if(_sm.IsHeavyAttack)
        {
            _sm.ChangeState(_sm.HeavyAttack);
        }
        else if(_sm.IsLightAttack)
        {
            _sm.ChangeState(_sm.LightAttack);
        }
    }

    private bool WaitForSeconds()
    {
        return Time.time > _sm.TimeToWaitAfterDash;
    }
}
