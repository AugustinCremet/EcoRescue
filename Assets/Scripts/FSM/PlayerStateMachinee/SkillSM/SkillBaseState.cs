using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBaseState : BaseState
{
    protected SkillSM _sm;
    protected Player _player;
    protected Animator _animator;
    protected FXManager _fxManager;
    protected SpeechManager _speechManager;
    protected float _targetTime;
    public SkillBaseState(SkillSM sm, Player player, Animator animator, FXManager fxManager, SpeechManager speechManager)
    {
        _sm = sm;
        _player = player;
        _animator = animator;
        _fxManager = fxManager;
        _speechManager = speechManager;
    }

    public override void Enter()
    {
        base.Enter();
        VerifyDash();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        VerifyDash();
    }

    private void VerifyDash()
    {
        if (_sm.MovementSM.GetCurrentState() is DashState && _sm.GetCurrentState() != _sm.Passive)
        {
            _animator.SetTrigger("passive");
            _animator.ResetTrigger("passive");
            _sm.ChangeState(_sm.Passive);
            return;
        }
    }
}
