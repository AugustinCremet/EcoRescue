using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnyKeyTransition : MonoBehaviour
{
    private UIManager _uiManager;

    private void Awake()
    {
        _uiManager = FindObjectOfType(typeof(UIManager)) as UIManager;
    }
    private void Update()
    {
        if (Input.anyKey)
        {
            _uiManager.StartMenuTransition();
            
            FindObjectOfType<SpeechManager>().StopSpeech("welcome");
        }
    }
}
