using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPU : PowerUp
{
    protected override void OnCollect(PlayerController _player)
    {
        _player.ActivateShield(timer);
        this.gameObject.SetActive(false);
    }
}
