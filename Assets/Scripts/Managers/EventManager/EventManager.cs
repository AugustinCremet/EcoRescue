using System;
using System.Collections.Generic;
using UnityEngine;

// Event Manager by :Bernardo Pacheco http://bernardopacheco.net/using-an-event-manager-to-decouple-your-game-in-unity
public class EventManager : MonoBehaviour
{
    private Dictionary<Events, Action<Dictionary<string, object>>> _eventDictionary;

    private static EventManager _eventManager;

    public static EventManager instance
    {
        get
        {
            if (!_eventManager)
            {
                _eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!_eventManager)
                {
                    Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                }
                else
                {
                    _eventManager.Init();

                    DontDestroyOnLoad(_eventManager);
                }
            }
            return _eventManager;
        }
    }

    void Init()
    {
        if (_eventDictionary == null)
        {
            _eventDictionary = new Dictionary<Events, Action<Dictionary<string, object>>>();
        }
    }

    public static void StartListening(Events eventName, Action<Dictionary<string, object>> listener)
    {
        Action<Dictionary<string, object>> thisEvent;

        if (instance._eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            instance._eventDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            instance._eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(Events eventName, Action<Dictionary<string, object>> listener)
    {
        if (_eventManager == null) return;
        Action<Dictionary<string, object>> thisEvent;
        if (instance._eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            instance._eventDictionary[eventName] = thisEvent;
        }
    }

    public static void TriggerEvent(Events eventName, Dictionary<string, object> message)
    {
        Action<Dictionary<string, object>> thisEvent;
        if (instance._eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            if(thisEvent != null)
                thisEvent.Invoke(message);
        }
    }
}
