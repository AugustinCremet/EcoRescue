using UnityEngine;

public class BossLogSoundController : MonoBehaviour
{
    private void Explosion()
    {
        FindObjectOfType<FXManager>().PlaySound("explosion" + Random.Range(1,4), gameObject);
    }
}
