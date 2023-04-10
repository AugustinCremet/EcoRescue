using System.Collections.Generic;
using UnityEngine;

public enum Positions
{
    ENTRANCE,
    EXIT
}

[RequireComponent(typeof(BoxCollider))]
public abstract class RoomDoor : MonoBehaviour
{
    [SerializeField] protected bool _loadsAnotherRoom = false;
    [SerializeField] protected Room _roomToLoad = null;
    protected Positions _posInNextRoom = Positions.ENTRANCE;
    protected bool _isLocked;

    protected BoxCollider _collider = null;

    virtual protected void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.isTrigger = true;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (!_loadsAnotherRoom) return;

        if (other.CompareTag("Player"))
        {
            if (!_isLocked)
            {
                EventManager.TriggerEvent(Events.SWITCH_ROOM,
                    new Dictionary<string, object> { { "roomToLoad", _roomToLoad }, { "position", _posInNextRoom } });
            }
        }
    }

    private void UnlockDoor(Dictionary<string,object> message)
    {
        _isLocked = false;
    }
    private void LockDoor(Dictionary<string, object> message)
    {
        _isLocked = true;
    }

    private void OnEnable()
    {
        EventManager.StartListening(Events.ROOM_CLEARED, UnlockDoor);
        EventManager.StartListening(Events.LOCK_CONNECTED_ROOMS, LockDoor);
    }
    private void OnDisable()
    {
        EventManager.StopListening(Events.ROOM_CLEARED, UnlockDoor);
        EventManager.StopListening(Events.LOCK_CONNECTED_ROOMS, LockDoor);
    }

    public bool LoadsAnotherRoom
    {
        get { return _loadsAnotherRoom; }
        set { _loadsAnotherRoom = value; }
    }
}