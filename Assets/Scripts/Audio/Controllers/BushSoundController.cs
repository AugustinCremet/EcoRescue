using UnityEngine;

public class BushSoundController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<FXManager>().PlaySound("walkthroughbush", gameObject);
    }
}
