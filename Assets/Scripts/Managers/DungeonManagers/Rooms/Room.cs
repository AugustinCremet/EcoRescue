using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public struct DoorTransform
{
    public Vector3 Position;
    public Quaternion Rotation;
}

public class Room : ScriptableObject
{
    [SerializeField] private DungeonManager _dungeonManager;
    [SerializeField] private string _name;
    [SerializeField] private List<DoorTransform> _doorTransform;
    [SerializeField] private bool _isLastRoom = false;
    private int _nbOfEnemiesLeft;
    private bool _wasCleared = false;

    public void UnlockDoors()
    {
        EventManager.TriggerEvent(Events.ROOM_CLEARED, null);
    }

    public void LockDoors()
    {
        EventManager.TriggerEvent(Events.LOCK_CONNECTED_ROOMS, null);
    }

    public void InitializeEnemies()
    {
        EventManager.TriggerEvent(Events.INITIALIZE_ENEMIES, null);
    }

    public void ResetRoom()
    {
        _nbOfEnemiesLeft = 0;
        _wasCleared = false;
    }

    public DungeonManager DungeonManager
    {
        set { _dungeonManager = value; }
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public List<DoorTransform> DoorTransform
    {
        get => _doorTransform;
        set => _doorTransform = value;
    }

    public int NbOfEnemiesAlive
    {
        get => _nbOfEnemiesLeft;
        set => _nbOfEnemiesLeft = value;
    }

    public bool WasCleared
    {
        get => _wasCleared;
        set => _wasCleared = value;
    }

    public bool IsLastRoom => _isLastRoom;
}

