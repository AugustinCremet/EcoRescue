using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDelay : MonoBehaviour
{
    [SerializeField] private bool _activate = false;
    [SerializeField] private Text _secondsText;
    [SerializeField] private int _initialSeconds = 60;
    
    private int _currentSeconds;

    private void OnEnable()
    {
        if (_activate)
            StartCoroutine(EffectActivated(_initialSeconds));
    }

    private void OnDisable()
    {
        _currentSeconds = _initialSeconds;
    }

    private IEnumerator EffectActivated(int timeSeconds)
    {
        _currentSeconds = timeSeconds;

        while(_currentSeconds != 0)
        {
            _currentSeconds--;
            _secondsText.text = _currentSeconds.ToString();
            yield return new WaitForSeconds(1);
        }
        gameObject.SetActive(false);
    }
}
