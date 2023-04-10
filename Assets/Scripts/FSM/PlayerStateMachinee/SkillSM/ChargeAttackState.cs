using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttackState : SkillBaseState
{
    private MakeParticlesEffect _effect;
    private float _timePassed;
    public ChargeAttackState(SkillSM sm, Animator animator, Player player, MakeParticlesEffect effect, FXManager fxManager, SpeechManager speechManager)
        : base(sm, player, animator, fxManager, speechManager)
    {
        _sm = sm;
        _animator = animator;
        _effect = effect;
    }

    public override void Enter()
    {
        base.Enter();
        _animator.SetTrigger("charge");
        _effect.SetDamage(_player.ChargeAttackDamage);
        _effect.StartParticles();
        _timePassed = 0;
        EventManager.TriggerEvent(Events.PLAYER_STAMINA_CHANGE, new Dictionary<string, object> { { "stamina", -2 } });
        _player.CurrentStamina -= 2;
        EventManager.TriggerEvent(Events.PLAYER_CHARGE_ATTACK, new Dictionary<string, object> { { "attack", "charge" } });

        if(Random.Range(0,20) == 0)
            _speechManager.PlaySpeech("malfunction" + Random.Range(0,6));
        else
            _speechManager.PlaySpeech("chargeattack" + Random.Range(1, 3));
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (_animator.GetCurrentAnimatorClipInfoCount(0) == 0)
            return;
        float clipLength = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        float clipSpeed = _animator.GetCurrentAnimatorStateInfo(0).speed;

        _timePassed += Time.deltaTime;

        if (_timePassed > clipLength / clipSpeed)
        {
            if(_sm.IsAiming)
            {
                _animator.SetTrigger("aim");
                _sm.ChangeState(_sm.Aim);
            }
            else
            {
                _animator.SetTrigger("passive");
                _sm.ChangeState(_sm.Passive);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        _animator.ResetTrigger("charge");
        _player.IsChargeReady = false;
    }
}
