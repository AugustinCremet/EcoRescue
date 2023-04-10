using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class DungeonManager : MonoBehaviour
{
    private Dungeon _activeDungeon;
    private Room _activeRoom;
    
    private List<GameObject> _activeEnemies = new List<GameObject>();
    private Queue<Room> _roomRestZone = new Queue<Room>();

    private GameObject _player;
    private GameObject _camera;
    private CinemachineVirtualCamera _cvm01;

    private GameObject _pathAnimalPrefab;
    private GameObject _pathAnimal;

    private MovementSM mSM;
    private SkillSM sSM;

    public void InitializeRooms()
    {
        foreach (Room room in _activeDungeon.Rooms)
        {
            room.DungeonManager = this;
            room.ResetRoom();
        }
    }

    public GameObject InitializePlayer(GameObject playerPrefab,GameObject cameraPrefab)
    {
        _player = Instantiate(playerPrefab, _activeRoom.DoorTransform[(int)Positions.ENTRANCE].Position, Quaternion.identity);
        _camera = Instantiate(cameraPrefab);
        _cvm01 = _camera.GetComponentInChildren<CinemachineVirtualCamera>();
        _cvm01.Follow = _player.transform;

        mSM = _player.GetComponent<MovementSM>();
        sSM = _player.GetComponent<SkillSM>();

        MoveToActiveDungeon(_player);
        MoveToActiveDungeon(_camera);

        return _player;
    }

    private void SwitchRooms(Dictionary<string, object> message)
    {
        var roomToLoad = (Room)message["roomToLoad"];
        var pos = (Positions)message["position"];

        if (_activeRoom == _activeDungeon.RestZone)
        {
            roomToLoad = _roomRestZone.Peek();
            _roomRestZone.Dequeue();
        }

        StartCoroutine(LoadNextLevel(roomToLoad, pos));
    }

    private IEnumerator LoadNextLevel(Room roomToLoad, Positions pos)
    {
        if (_pathAnimal != null)
        {
            Destroy(_pathAnimal);
        }

        ResetPlayerState();
        _activeEnemies.Clear();

        yield return new WaitForSeconds(0.5f);

        if (_activeRoom != null)
            SceneManager.UnloadSceneAsync(_activeRoom.Name);

        _activeRoom = roomToLoad;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(roomToLoad.Name, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null; 
        }

        EventManager.TriggerEvent(Events.FADE_OUT, null);

        if (!_activeRoom.WasCleared && _activeRoom != _activeDungeon.RestZone)
        {
            _activeRoom.LockDoors();
            _activeRoom.InitializeEnemies();
        }

        var door = roomToLoad.DoorTransform[(int)pos];

        _player.transform.position = door.Position;
        _player.transform.rotation = door.Rotation;
    }

    private void ResetPlayerState()
    {
        mSM.ResetState();
        sSM.ResetState();
        GameManager.instance.DisablePlayerInputs();
    }

    private void UnlockPlayerControls(Dictionary<string, object> message)
    {
        GameManager.instance.EnablePlayerInputs();
    }

    private void WinDungeon()
    {
        EventManager.TriggerEvent(Events.WIN, null);
    }

    private void OnEnemyKilled(Dictionary<string, object> message)
    {
        if (_activeRoom != null && _activeRoom.NbOfEnemiesAlive > 0)
            _activeRoom.NbOfEnemiesAlive--;

        if (_activeRoom.NbOfEnemiesAlive <= 0)
        {
            _activeRoom.WasCleared = true;

            if (_activeRoom.IsLastRoom)
                WinDungeon();
            else
                OnRoomCleared();
        }
    }

    private void OnEnemySpawned(Dictionary<string, object> message)
    {
        if (_activeRoom != null)
            _activeRoom.NbOfEnemiesAlive++;
    }

    private void OnRoomCleared()
    {
        EventManager.TriggerEvent(Events.ROOM_CLEARED, null);

        _pathAnimal = Instantiate(_pathAnimalPrefab, _player.transform.position, Quaternion.identity);

        MoveToActiveDungeon(_pathAnimal);

        var destination = _activeRoom.DoorTransform[(int)Positions.EXIT].Position;
        _pathAnimal.GetComponent<Rabbit>().SetDestination(destination);
    }

    private void MoveToActiveDungeon(GameObject obj)
    {
        SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByName(_activeDungeon.Name));
    }

    public void AddActiveEnemy(GameObject obj)
    {
        _activeEnemies.Add(obj);
    }

    public void RemoveActiveEnemy(GameObject obj)
    {
        _activeEnemies.Remove(obj);
    }

    private void OnEnable()
    {
        EventManager.StartListening(Events.SWITCH_ROOM, SwitchRooms);
        EventManager.StartListening(Events.ENEMY_KILLED, OnEnemyKilled);
        EventManager.StartListening(Events.ENEMY_SPAWNED, OnEnemySpawned);
        EventManager.StartListening(Events.UNLOCK_CONTROLS, UnlockPlayerControls);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.SWITCH_ROOM, SwitchRooms);
        EventManager.StopListening(Events.ENEMY_KILLED, OnEnemyKilled);
        EventManager.StopListening(Events.ENEMY_SPAWNED, OnEnemySpawned);
        EventManager.StopListening(Events.UNLOCK_CONTROLS, UnlockPlayerControls);
    }

    #region PROPRETIES
    public Dungeon ActiveDungeon
    {
        set { _activeDungeon = value; }
    }
    public Room ActiveRoom
    {
        get { return _activeRoom; }
        set { _activeRoom = value; }
    }
    public Queue<Room> RestZoneRooms
    {
        set { _roomRestZone = value; }
        get { return _roomRestZone; }
    }
    public GameObject Player
    {
        get { return _player; }
    }
    public GameObject PathAnimal
    {
        get { return _pathAnimalPrefab; }
        set { _pathAnimalPrefab = value; }
    }


    public List<GameObject> ActiveEnemies
    {
        get { return _activeEnemies; }
    }
    #endregion
}

