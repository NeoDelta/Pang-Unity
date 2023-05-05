using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunPU : PowerUp
{
    [SerializeField, Min(2)] protected float rateOfFire = 10f;

    protected override void OnCollect(PlayerController _player)
    {
        _player.ChangeRoF(timer, rateOfFire);
        this.gameObject.SetActive(false);
    }
}
