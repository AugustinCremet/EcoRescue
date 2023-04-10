using MalbersAnimations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UpdateEnemiesAnimation : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _agent;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        UpdateMvementAnimation();
    }

    private void UpdateMvementAnimation()
    {
        Vector3 relativeVelocity = transform.InverseTransformDirection(_agent.velocity);

        _animator.SetFloat("VelX", relativeVelocity.x);
        _animator.SetFloat("VelZ", relativeVelocity.z);
    }
}