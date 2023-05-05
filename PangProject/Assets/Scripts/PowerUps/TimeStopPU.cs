using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopPU : PowerUp
{   
    protected override void OnCollect(PlayerController _player)
    {
        GameManager.Instance.FreezeTime(timer);
        this.gameObject.SetActive(false);
    }
}
