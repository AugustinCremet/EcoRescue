using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death_Animator : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Destroy enemy");

        if (stateInfo.normalizedTime > 1) 
            Destroy(animator.transform.gameObject);
    }
}
