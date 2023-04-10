using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private int _damage = 1;

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        var hit = other.GetComponent<IDamageable>();
        
        if(hit == null)
            hit = other.transform.root.GetComponent<IDamageable>();

        FXManager _fxManager = FindObjectOfType<FXManager>();
        NeutralBehaviour parent = GetComponentInParent<NeutralBehaviour>();
        
        if (hit != null)
        {
            if (parent != null)
            {
                _fxManager.PlaySound("axehitfox" + Random.Range(1,7), parent.gameObject);
            }
            hit.TakeDamage(_damage);
        }
    }
}
