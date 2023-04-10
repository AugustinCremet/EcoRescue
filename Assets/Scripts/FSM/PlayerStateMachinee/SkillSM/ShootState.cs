using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootState : SkillBaseState
{
    private MakeParticlesEffect _effect;
    private float _timePassed;
    public ShootState(SkillSM sm, Animator animator, Player player, MakeParticlesEffect effect, FXManager fxManager, SpeechManager speechManager) : base(sm, player, animator, fxManager, speechManager)
    {
        _sm = sm;
        _effect = effect;
        _animator = animator;
        _fxManager = fxManager;
        _speechManager = speechManager;
    }

    public override void Enter()
    {
        base.Enter();
        _player.IsProjectileReady = false;
        _effect.SetDamage(_player.ProjectileDamage);
        _effect.SetMaxDistance(_player.ProjectileDistance);
        _effect.StartParticles();
        _animator.ResetTrigger("shoot");
        _animator.SetTrigger("shoot");
        _timePassed = 0f;
        EventManager.TriggerEvent(Events.PLAYER_STAMINA_CHANGE, new Dictionary<string, object> { { "stamina", -1 } });
        _player.CurrentStamina--;
        EventManager.TriggerEvent(Events.PLAYER_CHARGE_ATTACK, new Dictionary<string, object> { { "attack", "projectile" } });

        if(Random.Range(0,20) == 0)
            _speechManager.PlaySpeech("malfunction" + Random.Range(0,6));
        else
            _speechManager.PlaySpeech("projectiles" + Random.Range(1, 6));
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (_animator.GetCurrentAnimatorClipInfoCount(0) == 0)
            return;
        float clipLength = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        float clipSpeed = _animator.GetCurrentAnimatorStateInfo(0).speed;

        _timePassed += Time.deltaTime;

        if(_timePassed > clipLength / clipSpeed)
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
    }
}
