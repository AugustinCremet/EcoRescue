using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileState : BaseState
{
    private EnemySM _sm;

    public HostileState(EnemySM machine)
    {
        _sm = machine; 
    }

    public override void Enter()
    {
        base.Enter();

        _sm.Agent.speed = _sm.Enemy.HostileSpeed * (_sm.NeutralBT.SpeedMultiplier < 1 ? 1 : (_sm.NeutralBT.SpeedMultiplier > 1.5f ? 1.5f : _sm.NeutralBT.SpeedMultiplier));

        _sm.NeutralBT.enabled = false;
        _sm.HostileBT.enabled = true;
        _sm.HostileBT.UnPauseTree();
    }

    public override void UpdateLogic()
    {
        base.Enter();

        if (!_sm.IsHostile)
        {
            _sm.HostileBT.enabled = false;
            _sm.ChangeState(_sm.NeutralState);
        }
        
        if (_sm.Animator.GetBool("Hit"))
        {
            _sm.ChangeState(_sm.HitState);
        }

        if (_sm.Enemy.Health <= 0)
        {
            _sm.ChangeState(_sm.DeathState);
        }
    }

    public override void UpdatePhysics()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Enter();

        _sm.HostileBT.PauseTree();
    }
}
