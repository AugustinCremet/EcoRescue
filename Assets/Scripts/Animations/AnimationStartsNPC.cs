using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStartsNPC : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<NPCVendor>().ChatBubble.SetActive(false);
    }
}
