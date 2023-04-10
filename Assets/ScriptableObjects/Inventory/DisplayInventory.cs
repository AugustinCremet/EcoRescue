using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{    
    public InventoryObject _inventory;

    public int X_SPACE_BETWEEN_CONSUMABLE;
    public int X_START;
    public int Y_SPACE_BETWEEN_CONSUMABLE;
    public int Y_START;
    public int NUMBER_OF_COLUMN;

    public Dictionary<InventorySlot, GameObject> _consumableDisplayed = new Dictionary<InventorySlot, GameObject>();

    public virtual void Reset(Dictionary<string, object> message) { }
    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_CONSUMABLE * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_CONSUMABLE * (i / NUMBER_OF_COLUMN)), 0f);
    }
}
