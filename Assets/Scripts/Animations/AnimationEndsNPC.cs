using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEndsNPC : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<NPCVendor>().StartInteraction();
        animator.GetComponent<NPCVendor>().IsSpeaking = false;
    }
}
