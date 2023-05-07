using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float speed = 3f;
    [SerializeField, Min(1)] private float rateOfFire = 2f;
    [SerializeField] private Projectile projectile;
    [SerializeField] private Shield shield;

    private bool triggerPress = false;
    private bool shooting = false;
    private int ammo = 0;

    private Animator m_Animator;

    private Vector3 m_Movement = Vector3.zero;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        LevelManager.Instance.SetPlayer(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Ball")) return;

       LevelManager.Instance.OnLevelLose();
    }

    public void OnMove(InputAction.CallbackContext _ctx)
    {
        m_Movement = _ctx.ReadValue<Vector2>();
        m_Movement.y = 0f;
        this.transform.LookAt(transform.position + m_Movement);
    }

    public void OnShoot(InputAction.CallbackContext _ctx)
    {
        if (!projectile) return;

        if (_ctx.started)
            triggerPress = true;

        if (_ctx.canceled)
            triggerPress = false;
    }

    private IEnumerator ShootCo(float _time)
    {
        shooting = true;

        GameObject go = Instantiate(projectile.gameObject);
        go.transform.position = transform.position + Vector3.up;

        ammo = Mathf.Max(ammo - 1, 0);

        while (_time > 0)
        {
            yield return new WaitForEndOfFrame();
            _time -= Time.deltaTime;
        }

        shooting = false;
    }

    private void FixedUpdate()
    {
        Move();     
    }

    private void Update()
    {
        if (triggerPress && !shooting)
            StartCoroutine(ShootCo(1f / rateOfFire));
    }

    private void Move()
    {
        transform.position += m_Movement * speed * Time.fixedDeltaTime;
        m_Animator.SetFloat("Movement", Mathf.Abs(m_Movement.x));
    }

    public void ChangeRoF(int _ammo, float _rof)
    {
        ammo = _ammo;
        StopCoroutine(RateOfFireCo(_rof));
        StartCoroutine(RateOfFireCo(_rof));
    }

    public IEnumerator RateOfFireCo(float _rof)
    {
        //Change rof
        float baseRoF = rateOfFire;
        rateOfFire = _rof;

        while (ammo > 0f)
        {
            yield return new WaitForEndOfFrame();
        }

        //Resume Rof
        rateOfFire = baseRoF;
    }

    public void ActivateShield(float _time)
    {
        if (!shield) return;

        shield.gameObject.SetActive(true);
    }

    public void Dead()
    {
        m_Animator.SetTrigger("Death");
        transform.LookAt(Camera.main.transform);
    }


}
