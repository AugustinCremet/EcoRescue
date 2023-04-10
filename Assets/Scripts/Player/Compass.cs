using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    private Enemy _closestEnemy;
    private Image _compassImage;
    private Color _startColor;
    private Color _targetColor;

    private Coroutine _fade;

    private void Awake()
    {
        _compassImage = GetComponentInChildren<Image>();
        _startColor = new Color(_compassImage.color.r, _compassImage.color.g, _compassImage.color.b, 1f);
        _targetColor = new Color(_compassImage.color.r, _compassImage.color.g, _compassImage.color.b, 0f);
    }

    private void Update()
    {
        if (_closestEnemy == null || _closestEnemy.Dead)
        {
            if (_compassImage.color != _targetColor)
            {
                StopAllCoroutines();
                _compassImage.color = _targetColor;
            }

            return;
        }

        var toClosestEnemy = _closestEnemy.gameObject.transform.position - this.gameObject.transform.position;
        var dir = toClosestEnemy.normalized;
        dir.y = 0f;
        Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 20f);
    }

    public void UpdateCompass()
    {
        if (_fade != null)
            StopCoroutine(_fade);

        _closestEnemy = null;
        var playerPOS = this.gameObject.transform.position;
        var enemies = GameManager.instance.ActiveDungeonManager.ActiveEnemies;
        var closestDistSQR = Mathf.Infinity;

        foreach (var e in enemies)
        {
            if (e == null || e.GetComponentInChildren<Enemy>().Dead)
                continue;

            var distSQR = (e.transform.position - playerPOS).sqrMagnitude;

            if (distSQR < closestDistSQR)
            {
                closestDistSQR = distSQR;
                _closestEnemy = e.GetComponentInChildren<Enemy>();
            }
        }

        if (_closestEnemy != null)
        {
            _compassImage.color = _startColor;
            _fade = StartCoroutine(FadeCompass());
        }
    }

    private IEnumerator FadeCompass()
    {
        yield return new WaitForSeconds(5f);

        var timePassed = 0f;
        var duration = 1f;
        var startColor = _compassImage.color;
        var targetColor = new Color(_compassImage.color.r, _compassImage.color.g, _compassImage.color.b, 0f);

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;

            _compassImage.color = Color.Lerp(_startColor, _targetColor, timePassed / duration);

            yield return null;
        }

        _closestEnemy = null;
    }
}
