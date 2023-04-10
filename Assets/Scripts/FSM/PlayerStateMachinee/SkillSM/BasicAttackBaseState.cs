using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackBaseState : SkillBaseState
{
    protected MakeParticlesEffect _effect;
    protected HitBox _hitbox;
    protected bool _attackLight;
    protected bool _attackHeavy;
    protected float _timePassed;
    protected float _clipLength;
    protected float _clipSpeed;
    
    protected FXManager _fxManager;

    public BasicAttackBaseState(SkillSM sm, Animator animator, Player player, MakeParticlesEffect effect, HitBox hitbox, FXManager fxManager, SpeechManager speechManager)
        : base(sm, player, animator, fxManager, speechManager)
    {
        _animator = animator;
        _fxManager = fxManager;
        _speechManager = speechManager;
        _effect = effect;
        _hitbox = hitbox;
    }

    public override void Enter()
    {
        base.Enter();
        _animator.ResetTrigger("shoot");
        _animator.ResetTrigger("aim");
        _attackLight = false;
        _attackHeavy = false;
        _timePassed = 0f;
        _effect.StartParticles();
        _animator.SetFloat("Direction", 0f);
        if (_isUsingKeyboard)
            _sm.transform.LookAt(new Vector3(_sm.MousePositionObject.transform.position.x, _sm.transform.position.y, _sm.MousePositionObject.transform.position.z));
    }
    
    public override void UpdateLogic()
    {
        if (_sm.MovementSM.GetCurrentState() is DashState)
            _effect.EndParticles();
        base.UpdateLogic();
        
        if (_animator.GetCurrentAnimatorClipInfoCount(0) == 0)
            return;
        _clipLength = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        _clipSpeed = _animator.GetCurrentAnimatorStateInfo(0).speed;

        _timePassed += Time.deltaTime;

        if (_sm.IsLightAttack && _timePassed >= _clipLength / _clipSpeed / 2.0f)
        {
            _attackLight = true;
            _attackHeavy = false;
        }
        else if (_sm.IsHeavyAttack && _timePassed >= _clipLength / _clipSpeed / 2.0f)
        {
            _attackHeavy = true;
            _attackLight = false;
        }

        if (_timePassed >= _clipLength / _clipSpeed && _attackLight)
        {
            _sm.ChangeState(_sm.LightAttack);
        }
        else if (_timePassed >= _clipLength / _clipSpeed && _attackHeavy)
        {
            _sm.ChangeState(_sm.HeavyAttack);
        }

        if (_timePassed >= _clipLength / _clipSpeed && !_attackHeavy && !_attackLight)
        {
            _animator.SetTrigger("passive");
            _sm.ChangeState(_sm.Passive);
        }
    }
}
