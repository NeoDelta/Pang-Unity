using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunPU : PowerUp
{
    [SerializeField, Min(2)] protected float rateOfFire = 10f;
    [SerializeField, Min(1)] protected int ammo = 50;

    protected override void OnCollect(PlayerController _player)
    {
        _player.ChangeRoF(ammo, rateOfFire);
        this.gameObject.SetActive(false);
    }
}
