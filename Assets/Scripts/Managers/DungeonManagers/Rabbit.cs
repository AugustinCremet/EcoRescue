using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rabbit : MonoBehaviour
{
    [SerializeField] private float _maxDistance;

    private Transform _player;
    private NavMeshAgent _agent;
    private Animator _animator;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _player = GameManager.instance.Player.transform;
    }

    void Update()
    {
        UpdateAnimation();

        var distSqr = (_player.position - _agent.transform.position).sqrMagnitude;

        if (distSqr > _maxDistance * _maxDistance)
        {
            _agent.isStopped = true;
        }
        else
            _agent.isStopped = false;
    }

    private void UpdateAnimation()
    {
        _animator.SetFloat("Velocity", _agent.velocity.magnitude);
    }

    public void SetDestination(Vector3 destination)
    {
        _agent.SetDestination(destination);
    }
}
