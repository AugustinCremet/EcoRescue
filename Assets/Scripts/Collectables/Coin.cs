using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectable
{
    private int _coinValue = 10;

    protected override void OnCollected(Player player)
    {
        if (_collected) return;

        _collected = true;
        player.AddCoin(_coinValue);
    }
}
