using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : BaseState
{
    private EnemySM _sm;

    public HitState(EnemySM machine)
    {
        _sm = machine; 
    }

    public override void Enter()
    {
        base.Enter();
        _sm.Agent.velocity = Vector3.zero;
        _sm.RB.velocity = Vector3.zero;
        _sm.Agent.isStopped = true;
    }

    public override void UpdateLogic()
    {
        base.Enter();

        if (!_sm.IsStunt) 
        {
            _sm.ChangeState(_sm.HostileState);
        }

        if (_sm.Enemy.Health <= 0)
        {
            _sm.ChangeState(_sm.DeathState);
        }
    }

    public override void UpdatePhysics()
    {
        base.Enter();
        _sm.Agent.velocity = Vector3.zero;
        _sm.RB.velocity = Vector3.zero;
    }

    public override void Exit()
    {
        base.Enter();
        _sm.Agent.isStopped = false;
    }
}
