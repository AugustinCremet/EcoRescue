using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackState : BasicAttackBaseState
{
    public HeavyAttackState(SkillSM stateMachine, Animator animator, Player player, MakeParticlesEffect effect, HitBox hitbox, FXManager fxManager, SpeechManager speechManager) : base(stateMachine, animator, player, effect, hitbox, fxManager, speechManager) {}

    public override void Enter()
    {
        base.Enter();
        _hitbox.SetDamage(_player.HeavyAttackDamage);
        _animator.SetTrigger("heavyAttack");
        EventManager.TriggerEvent(Events.PLAYER_BASIC_ATTACK, new Dictionary<string, object> { { "attack", "heavy" } });
        //_fxManager.PlaySound("bite");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }

    public override void Exit()
    {
        base.Exit();
    }
}