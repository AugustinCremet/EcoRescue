using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageUI : MonoBehaviour
{
    private TMP_Text _text;
    private float fadeDuration = 2f;
    private Color targetColor; 
    private Color initialColor;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        targetColor = new Color(_text.color.r, _text.color.g, _text.color.b, 0f);
        initialColor = new Color(_text.color.r, _text.color.g, _text.color.b, 1f);

        StartCoroutine(FadeOut());
    }

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime);
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            _text.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeDuration);

            yield return null;
        }

        Destroy(gameObject);
    }
}
