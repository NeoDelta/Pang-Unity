using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float speed = 3f;
    [SerializeField] private Projectile projectile;
    private Rigidbody m_Rigidbody;

    private Vector3 m_Movement = Vector3.zero;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    [SerializeField] 
    public void OnMove(InputAction.CallbackContext _ctx)
    {
        m_Movement = _ctx.ReadValue<Vector2>();
        m_Movement.y = 0f;
    }

    public void OnShoot(InputAction.CallbackContext _ctx)
    {
        if (!projectile) return;

        if (!_ctx.performed) return;

        GameObject go = Instantiate(projectile.gameObject);
        go.transform.position = transform.position;
    }

    private void FixedUpdate()
    {
        transform.position += m_Movement * speed * Time.fixedDeltaTime;
    }
}
