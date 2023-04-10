using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlayerSkillTree : DisplaySkillTree
{
    //[SerializeField] private Transform _iconFolder;

    private GameObject[] _icons;

    private void Awake()
    {
        //for (int i = 0; i < _inventory._inventoryContainer.Count; i++)
        //{
        //    var obj = Instantiate(_inventory._inventoryContainer[i]._consumable._prefabInventory, Vector3.zero, Quaternion.identity, transform);
        //    obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
        //    obj.GetComponentInChildren<Text>().text = _inventory._inventoryContainer[i]._amount.ToString("n0");
        //}

        _icons = new GameObject[10];
    }

    private void ResetGame(Dictionary<string, object> message)
    {
        bool needReset = (bool)message["reset"];

        if (needReset == false) return;

        for (int i = 0; i < _inventory._inventoryContainer.Count; i++)
        {
            _inventory._inventoryContainer[i]._consumable.ResetSkill();
        }

        _inventory._inventoryContainer.Clear();

        if (_icons != null)
        {
            for (int i = 0; i < _icons.Length; i++)
            {
                Destroy(_icons[i]);
            }
        }
    }

    public void Update()
    {
        //UpdateDisplay(_inventory._inventoryContainer[_index]._consumable._prefabInventory);

        for (int i = 0; i < _inventory._inventoryContainer.Count; i++)
        {
            if (!_consumableDisplayed.ContainsKey(_inventory._inventoryContainer[i]))
            {
                var obj = Instantiate(_inventory._inventoryContainer[i]._consumable._prefabSkillTree, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                _consumableDisplayed.Add(_inventory._inventoryContainer[i], obj);
                _icons[i] = obj;
            }
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
