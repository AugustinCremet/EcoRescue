using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _damageTextPrefabs;
   
    private Canvas _damageCanvas;

    private void Awake()
    {
        _damageCanvas = GetComponent<Canvas>();
    }

    public void SpawnDamageUI(int damage)
    {
        var _damageText = Instantiate(_damageTextPrefabs, _damageCanvas.transform);
        Color color = new Color(1f, 1f - (damage * 0.01f), 0f, 1f);

        _damageText.color = color;
        _damageText.rectTransform.localPosition = new Vector3(Random.Range(-0.5f, 0.5f), 0f, 0f);

        var fontSize = Mathf.Clamp(5f * (damage * 0.001f), 0.3f, 0.5f);
        _damageText.fontSize = fontSize;
        _damageText.text = damage.ToString();
    }

    void LateUpdate()
    {
        Quaternion lookRotation = Quaternion.LookRotation(transform.forward - Camera.main.transform.forward);
        transform.rotation = lookRotation;
    }

}

