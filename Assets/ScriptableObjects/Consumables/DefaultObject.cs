using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Consumables/Default")]
public class DefaultObject : ConsumableObject
{
    public void Awake()
    {
        _type = ConsumableType.DEFAULT;
    }
}
