using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : RoomDoor
{
    protected override void Awake()
    {
        base.Awake();

        _posInNextRoom = Positions.ENTRANCE;
    }
}
