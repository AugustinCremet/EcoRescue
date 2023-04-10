using System.Collections;
using UnityEngine;

public class RepeatParticleWithDelay : MonoBehaviour
{
    [SerializeField] private float _repeatTime = 10f;

    private void Start()
    {
        StartCoroutine(RepeatParticle());
    }

    private IEnumerator RepeatParticle()
    {
        yield return new WaitForSeconds (_repeatTime);

        RestartParticle();
    }

    private void RestartParticle()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
            child.gameObject.SetActive(true);
            StartCoroutine(RepeatParticle());
        }
    }
}
