using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICallBuy : MonoBehaviour
{
    //private DisplayPlayerInventory _displayPlayerInventory;
    private DisplayMerchant _displayMerchant;

    [SerializeField] private ConsumableObject _consumable;
    private void Start()
    {
        //_displayPlayerInventory = FindObjectOfType(typeof(DisplayPlayerInventory)) as DisplayPlayerInventory;
        _displayMerchant = FindObjectOfType(typeof(DisplayMerchant)) as DisplayMerchant;
    }

    public void OnClick()
    {
        _displayMerchant.SaveBuyRef(_consumable);

        string consumable = _consumable.ToString().ToLower();
        if (consumable.IndexOf(" (") != -1)
            consumable = consumable.Remove(consumable.IndexOf(" ("));
        FindObjectOfType<SpeechManager>().StopAllConsumables();
        FindObjectOfType<SpeechManager>().PlaySpeech(consumable + "description");
    }
}
