using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Dungeon : ScriptableObject
{
    [SerializeField] private string _name;

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _cameraPrefab;
    [SerializeField] private GameObject _pathAnimalPrefab
        ;
    [SerializeField] private Room _startRoom;
    [SerializeField] private Room _tutorialRoom;
    [SerializeField] private Room _restZone;
    [SerializeField] private List<Room> _rooms = new List<Room>();
    [SerializeField] private List<Room> _restZoneQueue = new List<Room>();

    public string Name => _name;
    public GameObject PlayerPrefab => _playerPrefab;
    public GameObject CameraPrefab => _cameraPrefab;
    public GameObject PathAnimal => _pathAnimalPrefab;
    public Room StartRoom => _startRoom;
    public Room TutorialRoom => _tutorialRoom;
    public Room RestZone => _restZone;
    public List<Room> Rooms => _rooms;
    public List<Room> RestZoneQueue => _restZoneQueue;
}
