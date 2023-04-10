using System.Collections;
using UnityEngine;

public class UISoundController : MonoBehaviour
{
    void Start()
    {
       StartCoroutine(Welcome());
    }

    private IEnumerator Welcome()
    {
        yield return new WaitForSeconds(1f);
        
        FindObjectOfType<SpeechManager>().PlaySpeech("welcome");
    }
}
