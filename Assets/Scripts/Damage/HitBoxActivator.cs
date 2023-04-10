using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxActivator : MonoBehaviour
{
    [SerializeField] GameObject _normalHitBox;
    public void ActivateEnemyNormalHitBox()
    {
        _normalHitBox.SetActive(true);
    }

    public void DeactivateEnemyNormalHitBox()
    {
        _normalHitBox.SetActive(false);
    }
}
