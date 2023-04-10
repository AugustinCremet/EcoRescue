using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region SINGLETON
    private static GameManager gameManager;
    public static GameManager instance
    {
        get
        {
            if (!gameManager)
            {
                gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (!gameManager)
                {
                    Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                }
                else
                {

                    DontDestroyOnLoad(gameManager);
                }
            }
            return gameManager;
        }
    }
    #endregion

    [SerializeField] private Dungeon _activeDungeon;
    private DungeonManager _activeDungeonManager;
    private GameObject _player;
    private PlayerInputs _playerInputs;

    private void Awake()
    {
        _playerInputs = new PlayerInputs();
        _playerInputs.Enable();
    }

    private void LoadGame(Dictionary<string, object> message)
    {
        StartCoroutine(StartNewGame((bool)message["loadTutorial"]));
    }

    IEnumerator StartNewGame(bool loadTutorial)
    {
        SceneManager.LoadSceneAsync(_activeDungeon.Name, LoadSceneMode.Additive);

        var startRoom = _activeDungeon.StartRoom;

        if (loadTutorial)
        {
            startRoom = _activeDungeon.TutorialRoom;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(startRoom.Name, LoadSceneMode.Additive);

        while (!asyncLoad.isDone) 
        {
            yield return null;    
        }

        InitDungeonManager(startRoom, loadTutorial);
    }

    private void InitDungeonManager(Room startRoom, bool tutorial)
    {
        GameObject manager = new GameObject();
        manager.name = _activeDungeon.Name + " Manager";
        _activeDungeonManager = manager.AddComponent<DungeonManager>();

        SceneManager.MoveGameObjectToScene(manager, SceneManager.GetSceneByName(_activeDungeon.Name));

        _activeDungeonManager.ActiveDungeon = _activeDungeon;
        _activeDungeonManager.ActiveRoom = startRoom;
        _activeDungeonManager.PathAnimal = _activeDungeon.PathAnimal;

        foreach (var rooms in _activeDungeon.RestZoneQueue)
        {
            if (rooms == _activeDungeon.StartRoom && !tutorial)
                continue;

            _activeDungeonManager.RestZoneRooms.Enqueue(rooms);
        } 

        _activeDungeonManager.InitializeRooms();

        _activeDungeonManager.ActiveRoom.InitializeEnemies();
        _activeDungeonManager.ActiveRoom.LockDoors();
        _player = _activeDungeonManager.InitializePlayer(ActiveDungeon.PlayerPrefab, ActiveDungeon.CameraPrefab);

        if (tutorial)
        {
            EventManager.TriggerEvent(Events.START_HUD_TUTO, null);
            DisablePlayerInputs();
        }
        else
            EnablePlayerInputs();
    }

    public void DisablePlayerInputs()
    {
       _playerInputs.Menu.Disable();
       _playerInputs.Movement.Disable();
       _playerInputs.Skill.Disable();      
    }

    public void EnablePlayerInputs()
    {
        _playerInputs.Menu.Enable();
        _playerInputs.Movement.Enable();
        _playerInputs.Skill.Enable();
    }

    private void OnEnable()
    {
        EventManager.StartListening(Events.START_NEW_GAME, LoadGame);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.START_NEW_GAME, LoadGame);
    }

    public DungeonManager ActiveDungeonManager => _activeDungeonManager;
    public Dungeon ActiveDungeon => _activeDungeon;
    public GameObject Player => _player;
    public PlayerInputs Inputs => _playerInputs;
}
