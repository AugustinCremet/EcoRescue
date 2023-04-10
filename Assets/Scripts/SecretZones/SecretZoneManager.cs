using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class SecretZoneManager : MonoBehaviour
{
    private enum Rewards
    {
        UnlimitedStamina,
        OneConsumable,
        TwoConsumable,
        RewardsEnd
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

    [Header("North Limit")]
    [SerializeField] private Transform _colliderNorth;
    [Header("South Limit")]
    [SerializeField] private Transform _colliderSouth;
    [Header("East Limit")]
    [SerializeField] private Transform _colliderEast;
    [Header("West Limit")]
    [SerializeField] private Transform _colliderWest;
    
    private Player _player;
    private Animator _animator;
    private GameObject _secretUI;
    private UIHUDManager _uiHUD;

    private bool _hasGainedBuff;
    
    private int _maxStaminaOriginal;

    [Header("Consumables Prefab")]
    [SerializeField] private List<ConsumableObject> _consumables;

    [Header("Player Inventory")]
    [SerializeField] private InventoryObject _playerInventory;

    private void Awake()
    {
        _secretUI = FindObjectOfType<UIManager>().SecretZone;
        
        RandomLocation();
    }

    private void RandomLocation()
    {
        bool foundPosition = false;
        
        while(!foundPosition)
        {
            Vector3 pos = new Vector3(Random.Range(_colliderWest.localPosition.x, _colliderEast.localPosition.x), 5, Random.Range(_colliderNorth.localPosition.z, _colliderSouth.localPosition.z));
            transform.localPosition = pos;

            RaycastHit hit;
            if (Physics.Raycast(transform.localPosition, transform.TransformDirection(Vector3.down), out hit, 7f))
            {
                if(Math.Abs(hit.point.y) <= 0.00001f)
                {
                    pos.y = Math.Abs(hit.point.y) + 0.25f;
                    transform.localPosition = pos;
                    foundPosition = true;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyUp("e") && !_hasGainedBuff)
        {
            _player = FindObjectOfType<Player>();
            _animator = _player.GetComponent<Animator>();

            Destroy(GetComponent<RepeatParticleWithDelay>());
            transform.GetChild(0).gameObject.SetActive(false);

            _hasGainedBuff = true;
            
            _animator.SetBool("dig", true);

            switch (Random.Range(0, (int)Rewards.RewardsEnd))
            {
                case 0:
                    UnlimitedStamina(20f);
                    break;
                case 1:
                    Consumable(_consumables[(int)(Consumables)Random.Range(0, (int)Consumables.ConsumableEnd)], 1);
                    break;
                case 2:
                    Consumable(_consumables[(int)(Consumables)Random.Range(0, (int)Consumables.ConsumableEnd)], 2);
                    break;
            }
        }
    }

    private void UnlimitedStamina(float time)
    {
        _player.SetUnlimitedStamina(true);
        
        _secretUI.SetActive(true);
        _secretUI.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Unlimited Stamina for 20 seconds!!!";

        _uiHUD = FindObjectOfType<UIHUDManager>();
        _uiHUD.TriggerLoopFadeStamina();
        _uiHUD.AddStamina(_player.MaxStamina, true);

        StartCoroutine(StopDigging(0.3f));
        StartCoroutine(CloseUI());
        StartCoroutine(StaminaEndWarning(time - 1 - _uiHUD.StaminaFadeLength * 4));
        StartCoroutine(NormalStamina(time));
    }

    private void Consumable(ConsumableObject reward, int qty)
    {
        _secretUI.SetActive(true);
        
        string s = "reward";
        s = reward.ToString().IndexOf(" ") != -1 ? reward.ToString().Remove(reward.ToString().IndexOf(" ")) : reward.ToString();
        var words = Regex.Matches(s, @"([A-Z][a-z]+)").Cast<Match>().Select(m => m.Value);
        var withSpaces = string.Join(" ", words);

        _secretUI.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text
            = "You get " + qty + " " + withSpaces.ToString() + (qty > 1 ? "s" : "") + "!!!";
        
        _playerInventory.AddConsumable(reward, qty);
        
        _uiHUD = FindObjectOfType<UIHUDManager>();
        
        StartCoroutine(StopDigging(0.3f));
        StartCoroutine(CloseUI());
    }

    private IEnumerator StopDigging(float time)
    {
        yield return new WaitForSeconds(time);
        
        _animator.SetBool("dig", false);
    }

    private IEnumerator CloseUI()
    {
        yield return new WaitForSeconds(4f);
        
        _uiHUD.TriggerFadeOutSecretUI();

        yield return new WaitForSeconds(4f);
        
        _secretUI.SetActive(false);
    }
    
    private IEnumerator StaminaEndWarning(float time)
    {
        yield return new WaitForSeconds(time);
        
        _uiHUD.TriggerLoopFadeStamina(false);
    }
    
    private IEnumerator NormalStamina(float time)
    {
        yield return new WaitForSeconds(time);
        
        _player.SetUnlimitedStamina(false);

        Destroy(gameObject);
    }
}