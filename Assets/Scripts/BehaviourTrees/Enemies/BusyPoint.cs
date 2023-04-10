using System.Collections.Generic;
using UnityEngine;

public class BusyPoint : MonoBehaviour
{
    private List<int> _occupiedSlots = new List<int>();
    [SerializeField] private int _maxSlot = 4;

    public int MaxSlot => _maxSlot;
    public List<int> GetSlots => _occupiedSlots;


    public void AddSlot(int slot)
    {
        _occupiedSlots.Add(slot);
    }

    public void RemoveSlot(int slot)
    {
        _occupiedSlots.Remove(slot);
    }
}
