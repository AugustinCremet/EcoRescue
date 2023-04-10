using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIQuickUse : MonoBehaviour
{
    private PlayerInputs _playerInputs;
    private DisplayQuickUse _displayQuickUse;

    private void Awake()
    {
        _playerInputs = new PlayerInputs();

        _playerInputs.Consummable.Use1.started += OnUse1;
        _playerInputs.Consummable.Use2.started += OnUse2;
        _playerInputs.Consummable.Use3.started += OnUse3;
        _playerInputs.Consummable.Use4.started += OnUse4;
        _displayQuickUse = FindObjectOfType(typeof(DisplayQuickUse)) as DisplayQuickUse;

    }

    private void OnUse1(InputAction.CallbackContext obj)
    {
        EventManager.TriggerEvent(Events.PLAYER_CONSUMABLE, new Dictionary<string, object> { { "consumable", false } });
        obj.ReadValueAsButton();
        Consume(0);
    }
    private void OnUse2(InputAction.CallbackContext obj)
    {
        EventManager.TriggerEvent(Events.PLAYER_CONSUMABLE, new Dictionary<string, object> { { "consumable", false } });
        obj.ReadValueAsButton();
        Consume(1);
    }
    private void OnUse3(InputAction.CallbackContext obj)
    {
        EventManager.TriggerEvent(Events.PLAYER_CONSUMABLE, new Dictionary<string, object> { { "consumable", false } });
        obj.ReadValueAsButton();
        Consume(2);
    }
    private void OnUse4(InputAction.CallbackContext obj)
    {

        EventManager.TriggerEvent(Events.PLAYER_CONSUMABLE, new Dictionary<string, object> { { "consumable", false } });
        obj.ReadValueAsButton();
        Consume(3);
    }

    private void Consume(int slot)
    {
        EventManager.TriggerEvent(Events.PLAYER_CONSUMABLE, new Dictionary<string, object> { { "consumable", true } });

        if (_displayQuickUse._inventory._inventoryContainer.Count == 0) return;

        for (int i = 0; i < _displayQuickUse._inventory._inventoryContainer.Count; i++)
        {
            if (i == slot)
            {
                if (_displayQuickUse._inventory._inventoryContainer[slot]._consumable.UseConsumable() == true)
                {
                    _displayQuickUse._inventory._inventoryContainer[slot]._amount -= 1;
                }
            }
        }
        return;
    }

    private void OnEnable()
    {
        _playerInputs.Consummable.Enable();
    }
    private void OnDisable()
    {
        _playerInputs.Consummable.Disable();
    }
}
