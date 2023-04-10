using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : NPCVendor
{
    public override void StartInteraction()
    {
        base.StartInteraction();
        _uiManager.TrainerTransition(_chatBubble);
    }
}
