using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMove : MonoBehaviour
{
    [SerializeField] private float _time;
    [SerializeField] private float _moveSpeed = 10;
    [SerializeField] private bool _ableHit;
    [SerializeField] private float _hitDelay;
    [SerializeField] private GameObject _m_hitObject;
    [SerializeField] private float _maxLength;
    [SerializeField] private float _destroyTime2;
    [SerializeField] private LayerMask _canHitMask;
    [SerializeField] private LayerMask _enemyMask;

    private float _m_time;
    private float _m_time2;
    private GameObject _m_makedObject;
    private float m_scalefactor;
    private Vector3 _startingPos;
    private float _maxDistance;

    private void Start()
    {
        m_scalefactor = VariousEffectsScene.m_gaph_scenesizefactor;//transform.parent.localScale.x;
        _m_time = Time.time;
        _m_time2 = Time.time;
        _startingPos = transform.position;
    }

    public void SetMaxDistance(float maxDistance)
    {
        _maxDistance = maxDistance;
    }

    void LateUpdate()
    {
        //if (Time.time > _m_time + _time)
        //{
        //    Destroy(gameObject);
        //}

        transform.Translate(Vector3.forward * Time.deltaTime * _moveSpeed * m_scalefactor);
        float currentDistance = Vector3.Distance(_startingPos, transform.position);

        if (currentDistance > _maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.layer == LayerMask.NameToLayer("Enemy") &&
            !other.transform.gameObject.CompareTag("EnemyDamageHitbox"))
        {
            return;
        }
        else
        {
            StartParticle();
        }
    }

    private void StartParticle()
    {
        _m_makedObject = Instantiate(_m_hitObject, transform.position, Quaternion.identity).gameObject;
        Destroy(_m_makedObject, _destroyTime2);
        Destroy(gameObject, 0.1f);
    }
}
