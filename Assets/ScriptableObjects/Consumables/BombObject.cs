using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bomb Object", menuName = "Inventory System/Consumables/Bomb")]
public class BombObject : ConsumableObject
{
    public float _explosionRadius = 2f;
    public int _timeSecondsToExplode = 3;
    public int _damageValue = 200;
    [SerializeField] GameObject _bombPrefab;

    private DungeonManager _dungeonPlayer;

    public void Awake()
    {
        _type = ConsumableType.BOMB;
    }

    public override bool UseConsumable()
    {
        _dungeonPlayer = FindObjectOfType(typeof(DungeonManager)) as DungeonManager;
        _player = _dungeonPlayer.Player.GetComponent<Player>();

        Instantiate(_bombPrefab, new Vector3(_player.transform.position.x + 1, _player.transform.position.y, _player.transform.position.z), Quaternion.identity);
        return true;
    }
}
