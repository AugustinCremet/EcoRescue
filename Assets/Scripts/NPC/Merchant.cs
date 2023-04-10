using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : NPCVendor
{
    public override void StartInteraction()
    {
        base.StartInteraction();
        _uiManager.MerchantTransition(_chatBubble);
    }
}
