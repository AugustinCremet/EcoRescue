using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceDoor : RoomDoor
{
    protected override void Awake()
    {
        base.Awake();

        _posInNextRoom = Positions.EXIT; 
    }
}
