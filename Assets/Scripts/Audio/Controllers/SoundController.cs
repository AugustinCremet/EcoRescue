using UnityEngine;

public class SoundController : MonoBehaviour
{
    private void Footsteps()
    {
        FindObjectOfType<FXManager>().PlaySound("enemyfootsteps" + Random.Range(1,3), gameObject);
    }
    
    private void AttackSound()
    {
        FindObjectOfType<FXManager>().PlaySound("enemyattack" + Random.Range(1,5), gameObject);
    }
    
    private void DeathSound()
    {
        FindObjectOfType<FXManager>().PlaySound("enemydeath" + Random.Range(1,5), gameObject);
    }
    
    private void ShotgunSound()
    {
        FindObjectOfType<FXManager>().PlaySound("enemyshotgun" + Random.Range(1,3), gameObject);
    }
    
    private void ChoppingTreeSound()
    {
        FindObjectOfType<FXManager>().PlaySound("axehittree" + Random.Range(1, 16), gameObject);
    }
    
    private void AxeSwingSound()
    {
        FindObjectOfType<FXManager>().PlaySound("axehitair" + Random.Range(1, 5), gameObject);
    }

    private void BossLogThrowSound()
    {
        FindObjectOfType<FXManager>().PlaySound("bosslogthrow" + Random.Range(1, 5), gameObject);
    }
}
