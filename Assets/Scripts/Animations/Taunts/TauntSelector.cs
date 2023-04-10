using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntSelector : StateMachineBehaviour
{
    [SerializeField] private int _nbAnimationClips;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int randomClip = Random.Range(0, _nbAnimationClips);
        animator.SetInteger("TauntRandom", randomClip);
    }
}
