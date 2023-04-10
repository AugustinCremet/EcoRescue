using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Skill Tree", menuName = "Inventory System/SkillTree")]
public class SkillTreeObject : ScriptableObject
{
    public List<SkillSlot> _inventoryContainer = new List<SkillSlot>();

    public void AddSkill(SkillObject skill, int amount)
    {
        for (int i = 0; i < _inventoryContainer.Count; i++)
        {
            if (_inventoryContainer[i]._consumable == skill)
            {
                _inventoryContainer[i]._consumable.UnlockSkill();
                return;
            }
        }
        _inventoryContainer.Add(new SkillSlot(skill, amount));
    }

    public void SwapSkill(SkillTreeObject fromInventory, SkillTreeObject toInventory, SkillObject skill)
    {
        for (int i = 0; i < fromInventory._inventoryContainer.Count; i++)
        {
            if (fromInventory._inventoryContainer[i]._consumable == skill)
            {
                toInventory.AddSkill(skill, fromInventory._inventoryContainer[i]._amount);

                //fromInventory._inventoryContainer.RemoveAt(i);
                return;
            }
        }
    }
}

[System.Serializable]
public class SkillSlot
{
    public SkillObject _consumable;
    public int _amount;

    public SkillSlot(SkillObject consumable, int amount)
    {
        _consumable = consumable;
        _amount = amount;
    }

    public void AddAmount(int value)
    {
        _amount += value;
    }
}
