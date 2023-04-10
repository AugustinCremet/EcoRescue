using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralState : BaseState
{
    private EnemySM _sm;

    public NeutralState (EnemySM machine)
    {
        _sm = machine; 
    }

    public override void Enter()
    {
        base.Enter();

        _sm.Agent.speed = _sm.Enemy.NeutralSpeed * _sm.NeutralBT.SpeedMultiplier;
        _sm.HostileBT.enabled = false;
        _sm.NeutralBT.enabled = true;
        _sm.NeutralBT.UnPauseTree();
    }

    public override void UpdateLogic()
    {
        base.Enter();

        if (_sm.IsHostile)
        {
            _sm.ChangeState(_sm.HostileState);
        }
        else if (_sm.Animator.GetBool("Working"))
        {
            _sm.ChangeState(_sm.WorkState);
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
        _sm.NeutralBT.PauseTree();
        _sm.NeutralBT.enabled = false;
    }
}
