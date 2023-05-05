using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class PowerUp : MonoBehaviour
{
    [SerializeField, Min(0)] protected float timer = 5f;

    private Rigidbody m_Rigidbody;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.gameObject.CompareTag("Player"))
            return;

        OnCollect(collision.collider.GetComponent<PlayerController>());
    }

    protected virtual void OnCollect(PlayerController _player)
    {
        Destroy(this.gameObject);
    }

    public void EnableGravity(bool _enable = true)
    {
        m_Rigidbody.useGravity = _enable;
        StartCoroutine(TimeCo(timer));
    }

    public IEnumerator TimeCo(float _time)
    {

        while (_time > 0)
        {
            yield return new WaitForEndOfFrame();
            _time -= Time.deltaTime;
        }

        Destroy(this);
    }
}
