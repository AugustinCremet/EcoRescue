using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDamage : MonoBehaviour
{
    [SerializeField] private bool _isExploding;
    [SerializeField] private ParticleSystem _explosionParticles;
    [SerializeField] private string _target;
    [SerializeField] private int _damage = 200;

    private void OnEnable()
    {
        _explosionParticles.Play();
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            other.GetComponent<IDamageable>().TakeDamage(1);
        }
        else if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_damage);
        }
    }
}
