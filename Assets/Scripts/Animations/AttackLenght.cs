    using UnityEngine;
using UnityEngine.AI;

public class AttackLenght : StateMachineBehaviour
{
    private HostileBehaviour _hb;
    private NavMeshAgent _agent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_hb == null)
            _hb = animator.GetComponent<HostileBehaviour>();

        _hb.ActionEnd = stateInfo.length + Time.time;

        if (_agent == null)
            _agent = animator.gameObject.GetComponent<NavMeshAgent>();

        _agent.isStopped = true;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent.isStopped = false;
    }
}
