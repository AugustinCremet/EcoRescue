using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    private MovementSM _sm;

    public IdleState(MovementSM machine, Animator anim)
    {
        _sm = machine;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (_sm.IsWalking)
        {
            _sm.ChangeState(_sm.WalkState); 
        }

        //if (_sm.IsDashing)
        //{
        //    _sm.ChangeState(_sm.DashState);
        //}
    }
}
