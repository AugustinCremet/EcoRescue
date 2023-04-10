using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    [SerializeField] private GameObject _bombObject;
    [SerializeField] private GameObject _damageEffect;
    [SerializeField] private Text _secondsText;
    [SerializeField] private int _initialSeconds = 3;

    private int _currentSeconds;

    private void OnEnable()
    {
        _damageEffect.SetActive(false);
        StartCoroutine(EffectActivated(_initialSeconds));
    }

    private void OnDisable()
    {
        _currentSeconds = _initialSeconds;
    }

    private IEnumerator EffectActivated(int timeSeconds)
    {
        _currentSeconds = timeSeconds;

        while (_currentSeconds != 0)
        {
            _currentSeconds--;
            _secondsText.text = _currentSeconds.ToString();
            yield return new WaitForSeconds(1);
        }
        _bombObject.SetActive(false);
        _damageEffect.SetActive(true);
        
        FindObjectOfType<FXManager>().PlaySound("bomb" + Random.Range(1,4), FindObjectOfType<Player>().gameObject);

        int explosionTime = 2;

        while (explosionTime != 0)
        {
            explosionTime--;
            yield return new WaitForSeconds(1);
        }
        

        Destroy(gameObject);
    }
}
