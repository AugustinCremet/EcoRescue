using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] private Material _mat;
    private float timePassed = 0f;
    private float duration = 0.5f;

    private Color _startColor = Color.white;
    private Color _targetColor = Color.red;
    private Coroutine _coroutine;

    private void Awake()
    {
        _mat.color = _startColor;
    }

    private void Damage(Dictionary<string,object> message)
    {
        if (_coroutine != null)      
            StopCoroutine(_coroutine);
        
        timePassed = 0f;
        _coroutine = StartCoroutine(ChangeFoxMaterial());
    }

    private IEnumerator ChangeFoxMaterial()
    {
        _mat.color = _targetColor;

        while (timePassed <= duration)
        {
            timePassed += Time.deltaTime;

            _mat.color = Color.Lerp(_targetColor, _startColor, timePassed / duration);

            yield return null;
        }
    }

    private void OnEnable()
    {
        EventManager.StartListening(Events.PLAYER_TAKE_DAMAGE, Damage);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.PLAYER_TAKE_DAMAGE, Damage);
    }
}
