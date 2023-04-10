using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelfDestruct : MonoBehaviour
{
    [SerializeField] Text _amount;

    void Update()
    {
        if(_amount.text == "0")
        {
            Destroy(gameObject);
        }
    }
    private void ResetGame(Dictionary<string, object> message)
    {
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        EventManager.StartListening(Events.RESET, ResetGame);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.RESET, ResetGame);
    }
}
