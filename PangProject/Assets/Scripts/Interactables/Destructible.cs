using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Destructible : MonoBehaviour
{
    enum OnCollision
    {
        Destroy,
        Disable
    }

    [SerializeField] private OnCollision onCollision;

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Projectile")) return;

        if (onCollision == OnCollision.Destroy)
            Destroy(this.gameObject);
        else
            this.gameObject.SetActive(false);
    }
}
