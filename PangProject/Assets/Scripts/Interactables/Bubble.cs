using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : Destructible
{
    [SerializeField] private PowerUp powerUp;

    private void Awake()
    {
        if (!powerUp)
            this.gameObject.SetActive(false);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (!collision.collider.CompareTag("Projectile")) return;

        OnPop();
    }

    private void OnPop()
    {
        powerUp.EnableGravity();
    }
}
