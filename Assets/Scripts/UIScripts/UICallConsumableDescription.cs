using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UICallConsumableDescription : MonoBehaviour
{
    private DisplayPlayerInventory _displayPlayerInventory;
    private DisplayQuickUse _displayQuickUse;
    private UIManager _manager;
    [SerializeField] private ConsumableObject _consumable;

    private void Awake()
    {
        _displayPlayerInventory = FindObjectOfType(typeof(DisplayPlayerInventory)) as DisplayPlayerInventory;
        _displayQuickUse = FindObjectOfType(typeof(DisplayQuickUse)) as DisplayQuickUse;
        _manager = FindObjectOfType(typeof(UIManager)) as UIManager;
    }

    public void OnClick()
    {
        _manager.OpenQuickUse();
        _displayQuickUse = FindObjectOfType(typeof(DisplayQuickUse)) as DisplayQuickUse;
        _manager.CloseQuickUse();

        _displayPlayerInventory.DisplayOverview(_consumable);
        _displayQuickUse.SaveEquipRef(_consumable);
        
        FindObjectOfType<SpeechManager>().StopAllConsumables();

        string s = _consumable.ToString();
        s = s.IndexOf(" ") != -1 ? s.Remove(s.IndexOf(" ")) : s;
        s = s.ToLower() + "description";

        FindObjectOfType<SpeechManager>().PlaySpeech(s);
    }

    private enum Consumables
    {
        Bomb,
        SmallPotion,
        StaminaPotion,
        PowerPotion,
        BigPotion,
        ConsumableEnd
    }
}
