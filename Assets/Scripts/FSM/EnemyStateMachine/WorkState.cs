using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkState : BaseState
{
    private EnemySM _sm;
    private Vector3 _lookDir;
    private float _dotThreshold = 0.98f;

    public WorkState(EnemySM machine)
    {
        _sm = machine; 
    }

    public override void Enter()
    {
        base.Enter();

        _sm.Agent.isStopped = true;
        _sm.NeutralBT.enabled = true;
        _sm.NeutralBT.UnPauseTree();

        Collider[] colliders = Physics.OverlapSphere(_sm.transform.position, 5f);

        foreach (var c in colliders)
        {
            if (c.CompareTag("EnemyWorkplace"))
            {
                _lookDir = (c.transform.position - _sm.transform.position).normalized;
                break;
            }
        }
    }

    public override void UpdateLogic()
    {
        base.Enter();

        if (_sm.IsHostile)
        {
            _sm.ChangeState(_sm.HostileState);
        }
        else if (!_sm.Animator.GetBool("Working")) 
        {
            _sm.ChangeState(_sm.NeutralState);
        }

        if (_sm.Enemy.Health <= 0)
        {
            _sm.ChangeState(_sm.DeathState);
        }
    }

    public override void UpdatePhysics()
    {
        base.Enter();

        var dot = Vector3.Dot(_lookDir, _sm.transform.forward);

        if (dot < _dotThreshold)
        {
            if (_lookDir == Vector3.zero) return;

            Quaternion rotation = Quaternion.LookRotation(_lookDir, Vector3.up);
            _sm.transform.rotation = Quaternion.RotateTowards(_sm.transform.rotation, rotation, 400f * Time.deltaTime);
        }
    }

    public override void Exit()
    {
        base.Enter();
        _sm.Agent.isStopped = false;
    }
}
