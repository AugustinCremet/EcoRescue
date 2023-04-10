using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChainsawSoundController : MonoBehaviour
{
    private float _waitTime = 45f;
    private void Start()
    {
        StartCoroutine(PlayRandomSound());
    }

    private IEnumerator PlayRandomSound()
    {
        yield return new WaitForSeconds(_waitTime);
        
        FindObjectOfType<AmbientManager>().PlayAmbient("Chainsaw" + Random.Range(1, 12));
        StartCoroutine(PlayRandomSound());
        _waitTime = Random.Range(45f, 75f);
    }
}
