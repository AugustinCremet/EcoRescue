using System.Collections;
using UnityEngine;

public class BossLogExplosion : MonoBehaviour
{
    [SerializeField] private GameObject _explosion;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            
            _explosion.GetComponent<ParticleSystem>().Play();

            FindObjectOfType<FXManager>().PlaySound("explosion", gameObject);
            
            if (Vector3.Distance(FindObjectOfType<Player>().transform.position, transform.position) < 2f)
                FindObjectOfType<Player>().GetComponent<IDamageable>().TakeDamage(3);
            
            StartCoroutine(DestroyObject());
        }
    }
    
    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds (1f);
        
        Destroy(gameObject);
    }
}
