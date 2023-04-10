using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> _inventoryContainer = new List<InventorySlot>();
    public void AddConsumable(ConsumableObject consumable, int amount)
    {
        foreach (var slot in _inventoryContainer)
        {
            if (slot._consumable == consumable)
            {
                slot.AddAmount(amount);
                return;
            }
        }

        foreach (var empty in _inventoryContainer)
        {
            if (empty._consumable == null)
            {
                empty._consumable = consumable;
                empty._amount = amount;
                return;
            }
        }
    }

    public void SwapConsumable(InventoryObject fromInventory, InventoryObject toInventory, ConsumableObject consumable)
    {
        for (int i = 0; i < fromInventory._inventoryContainer.Count; i++)
        {
            if (fromInventory._inventoryContainer[i]._consumable == consumable)
            {
                toInventory.AddConsumable(consumable, fromInventory._inventoryContainer[i]._amount);
                fromInventory._inventoryContainer[i]._amount = 0;
                return;
            }
        }
    }

    public int GetAmount(InventoryObject inventory, ConsumableObject consumable)
    {
        for (int i = 0; i < inventory._inventoryContainer.Count; i++)
        {
            if (inventory._inventoryContainer[i]._consumable == consumable)
            {
                return inventory._inventoryContainer[i]._amount;
            }
        }
        return 0;
    }
}

[System.Serializable]
public class InventorySlot
{
    public ConsumableObject _consumable;
    public int _amount;

    public InventorySlot(ConsumableObject consumable, int amount)
    {
        _consumable = consumable;
        _amount = amount;
    }

    public void AddAmount(int value)
    {
        _amount += value;
    }
}
