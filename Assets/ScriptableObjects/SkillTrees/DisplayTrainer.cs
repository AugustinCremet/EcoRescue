using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTrainer : DisplaySkillTree
{
    [SerializeField] private Text _buyText;
    [SerializeField] private SkillTreeObject _trainerSkillTree;
    [SerializeField] private SkillTreeObject _playerSkillTree;

    private SkillObject _skill;
    private Player _player;

    private GameObject _icon;

    [SerializeField] private SkillSlot _grand;
    [SerializeField] private SkillSlot _quick;
    [SerializeField] private SkillSlot _radius;
    [SerializeField] private SkillSlot _rocket;
    [SerializeField] private SkillSlot _extra;
    [SerializeField] private SkillSlot _skate;
    [SerializeField] private SkillSlot _cat;
    [SerializeField] private SkillSlot _eat;
    [SerializeField] private SkillSlot _springspring;
    [SerializeField] private SkillSlot _springspringspring;

    private void Awake()
    {
        _player = FindObjectOfType(typeof(Player)) as Player;
    }

    private void ResetGame(Dictionary<string, object> message)
    {
        bool needReset = (bool)message["reset"];

        if (needReset == false) return;

        _inventory._inventoryContainer.Clear();

        _inventory._inventoryContainer.Add(_grand);
        _inventory._inventoryContainer.Add(_quick);
        _inventory._inventoryContainer.Add(_radius);
        _inventory._inventoryContainer.Add(_rocket);
        _inventory._inventoryContainer.Add(_extra);
        _inventory._inventoryContainer.Add(_skate);
        _inventory._inventoryContainer.Add(_cat);
        _inventory._inventoryContainer.Add(_eat);
        _inventory._inventoryContainer.Add(_springspring);
        _inventory._inventoryContainer.Add(_springspringspring);

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        for (int i = 0; i < _inventory._inventoryContainer.Count; i++)
        {
            if (!_consumableDisplayed.ContainsKey(_inventory._inventoryContainer[i]))
            {
                var obj = Instantiate(_inventory._inventoryContainer[i]._consumable._prefabShop, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                _consumableDisplayed.Add(_inventory._inventoryContainer[i], obj);
            }
        }
    }

    public void SaveBuyRef(SkillObject skill, GameObject icon)
    {
        _skill = skill;
        _icon = icon;
    }

    public void BuyDisplay()
    {
        _player = FindObjectOfType(typeof(Player)) as Player;

        if (_player.NbOfCoins >= _skill._price)
        {
            _trainerSkillTree.SwapSkill(_trainerSkillTree, _playerSkillTree, _skill);
            Destroy(_icon);
            _skill.UnlockSkill();

            for (int i = 0; i < _inventory._inventoryContainer.Count; i++)
            {
                if (_inventory._inventoryContainer[i]._consumable == _skill)
                {
                    _consumableDisplayed.Remove(_inventory._inventoryContainer[i]);
                }
            }

            _player.NbOfCoins -= _skill._price;
            EventManager.TriggerEvent(Events.PLAYER_COIN_CHANGE, new Dictionary<string, object> { { "coin", _player.NbOfCoins } });
        }

    }

    private void OnEnable()
    {
        EventManager.StartListening(Events.RESET, ResetGame);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.RESET, ResetGame);
    }
}
