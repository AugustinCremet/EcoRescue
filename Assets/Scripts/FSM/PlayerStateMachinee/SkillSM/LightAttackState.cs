using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackState : BasicAttackBaseState
{
    public LightAttackState(SkillSM stateMachine, Animator animator, Player player, MakeParticlesEffect effect, HitBox hitbox, FXManager fxManager, SpeechManager speechManager)
        : base(stateMachine, animator, player, effect, hitbox, fxManager, speechManager) {}

    public override void Enter()
    {
        base.Enter();
        _hitbox.SetDamage(_player.LightAttackDamage);
        _animator.SetTrigger("lightAttack");
        EventManager.TriggerEvent(Events.PLAYER_BASIC_ATTACK, new Dictionary<string, object> { { "attack", "light" } });
        //_fxManager.PlaySound("claw");
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
