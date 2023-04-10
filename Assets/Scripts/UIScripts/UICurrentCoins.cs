using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICurrentCoins : MonoBehaviour
{
    private Player _player;

    //private void Awake()
    //{
    //    _player = FindObjectOfType(typeof(Player)) as Player;
    //}

    //public void UpdateCurrentCoins(Dictionary<string, object> message)
    //{
    //    gameObject.FindComponent<Text>().text = _player.NbOfCoins.ToString("n0");
    //}

    //private void OnEnable()
    //{
    //    if( _player != null ) gameObject.FindComponent<Text>().text = _player.NbOfCoins.ToString("n0");
    //    EventManager.StartListening(Events.PLAYER_COIN_CHANGE, UpdateCurrentCoins);
    //}

    //private void OnDisable()
    //{
    //    EventManager.StopListening(Events.PLAYER_COIN_CHANGE, UpdateCurrentCoins);
    //}

    private void Update()
    {
        _player = FindObjectOfType(typeof(Player)) as Player;
        if( _player != null )
        {
            _player = FindObjectOfType(typeof(Player)) as Player;
            gameObject.FindComponent<Text>().text = _player.NbOfCoins.ToString("n0");
        }
    }
}
