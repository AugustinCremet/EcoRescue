using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion Object", menuName = "Inventory System/Consumables/Potion")]
public class PotionObject : ConsumableObject
{
    public int _restorePointValue;
    private DungeonManager _dungeonPlayer;

    public void Awake()
    {
        _type = ConsumableType.POTION;

    }

    public override bool UseConsumable() 
    {
        _dungeonPlayer = FindObjectOfType(typeof(DungeonManager)) as DungeonManager;
        _player = _dungeonPlayer.Player.GetComponent<Player>();

        if (_player.CurrentHealth < _player.MaxHealth)
        {
            EventManager.TriggerEvent(Events.PLAYER_HEALTH_CHANGE, new Dictionary<string, object> { { "health", _restorePointValue } });
            return true;
        }
        else return false;
    }
}
