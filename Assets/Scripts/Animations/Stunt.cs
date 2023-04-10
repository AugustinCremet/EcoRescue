using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Stunt : StateMachineBehaviour
{
    private EnemySM _sm;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_sm == null)
        {
            _sm = animator.gameObject.GetComponent<EnemySM>();
        }

        _sm.IsStunt = true;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _sm.IsStunt = false;
    }
}
