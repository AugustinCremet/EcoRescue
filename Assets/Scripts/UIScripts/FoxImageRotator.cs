using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxImageRotator : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 45f;

    private IEnumerator RotateFox()
    {
        while (true)
        {
            this.transform.Rotate(0,0, _rotationSpeed * Time.deltaTime);

            yield return null;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(RotateFox());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
