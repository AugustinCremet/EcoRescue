using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stamina Potion Object", menuName = "Inventory System/Consumables/Stamina")]
public class StaminaObject : ConsumableObject
{
    private DungeonManager _dungeonPlayer;
    public void Awake()
    {
        _type = ConsumableType.STAMINA;
    }

    public override bool UseConsumable() 
    {
        _dungeonPlayer = FindObjectOfType(typeof(DungeonManager)) as DungeonManager;
        _player = _dungeonPlayer.Player.GetComponent<Player>();

        if (_player.CurrentStamina < _player.MaxStamina)
        {
            EventManager.TriggerEvent(Events.PLAYER_STAMINA_CHANGE, new Dictionary<string, object> { { "stamina", _player.MaxStamina } });
            return true;
        }
        else return false;
    }
}
