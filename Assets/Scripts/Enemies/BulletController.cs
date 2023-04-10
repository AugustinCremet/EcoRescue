using System;
using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Range(0,10)]
    [SerializeField] private int _damage = 1;
    private float _speed = 25f;

    private void Awake()
    {
        StartCoroutine(AutoDestroy());
    }

    private IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds (1.5f);
        
        Destroy(gameObject);
    }

    private void LateUpdate()
    {
        transform.Translate(transform.forward * _speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        // To be Updated
        //if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<Player>().TakeDamage(_damage);
            //Destroy(gameObject);
        }
    }
}
