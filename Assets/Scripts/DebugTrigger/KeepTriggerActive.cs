using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeepTriggerActive : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(KeepActive());
    }

    private IEnumerator KeepActive()
    { 
        yield return new WaitForSeconds (20f);
        transform.GetChild(0).gameObject.SetActive(true);
        Debug.ClearDeveloperConsole();
        StartCoroutine(KeepActive());
    }
}
