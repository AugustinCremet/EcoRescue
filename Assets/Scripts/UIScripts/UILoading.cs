using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoading : MonoBehaviour
{
    private UIManager _uiManager;
    [SerializeField] float _secondsDelay = 2f;

    void Start()
    {
        _uiManager = FindObjectOfType(typeof(UIManager)) as UIManager;
        
    }

    public void OnEnable()
    {
        StartCoroutine(Loader());
    }

    IEnumerator Loader()
    {
        yield return new WaitForSeconds(_secondsDelay);
        _uiManager.LoadingTransition();
        _uiManager.OpenHUD();
    }
}
