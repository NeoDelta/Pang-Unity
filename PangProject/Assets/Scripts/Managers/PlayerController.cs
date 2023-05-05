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

        if (!_ctx.performed) return;

        if (!shooting) StartCoroutine(ShootCo(1f/rateOfFire));
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

        while (ammo > 0)
        {
            yield return new WaitForEndOfFrame();
        }

        //Resume Rof
        rateOfFire = baseRoF;
    }

    public void ActivateShield(float _time)
    {
        if (!shield) return;

        //StopCoroutine(ShieldCo(_time));
        //StartCoroutine(ShieldCo(_time));

        shield.gameObject.SetActive(true);
    }

    public IEnumerator ShieldCo(float _time)
    {
        //Activate shield
        shield.gameObject.SetActive(true);

        while (_time > 0)
        {
            yield return new WaitForEndOfFrame();
            _time -= Time.deltaTime;
        }

        //Deactivate shield
        shield.gameObject.SetActive(false);
    }


}
