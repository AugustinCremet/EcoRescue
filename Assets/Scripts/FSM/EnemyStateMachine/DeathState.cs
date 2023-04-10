using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : BaseState
{
    private EnemySM _sm;

    public DeathState(EnemySM machine)
    {
        _sm = machine;
    }

    public override void Enter()
    {
        base.Enter();
        _sm.Agent.isStopped = true;
        _sm.Agent.velocity = Vector3.zero;
        _sm.RB.velocity = Vector3.zero;
        _sm.Animator.SetTrigger("Death");      
    }
}
