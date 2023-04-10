using UnityEngine;
using UnityEngine.UI;

public class DebugLogTrigger : MonoBehaviour
{
    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError(_text.text);
        gameObject.SetActive(false);
    }
}
