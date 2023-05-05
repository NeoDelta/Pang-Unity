using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    [SerializeField] private float initialForce = 2f;
    [SerializeField] private Vector3 initialDirection = Vector3.right;
    [SerializeField] private List<Ball> splitsInto = new List<Ball>();

    private Rigidbody m_Rigidbody;
    private Vector3 previousVelocity = Vector3.right;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        GameManager.Instance.timeFreeze.AddListener(Freeze);
        GameManager.Instance.timeUnFreeze.AddListener(UnFreeze);

        LevelManager.Instance.AddBall(this);

        m_Rigidbody.AddForce(initialDirection * initialForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Projectile")) return;

        Split();
        GameManager.Instance.UpdateScore(200f);
        LevelManager.Instance.RemoveBall(this);
        
        Destroy(collision.collider.gameObject);
        Destroy(this.gameObject);
    }

    private void Split()
    {
        for(int i = 0; i < splitsInto.Count; i++)
        {
            GameObject go = Instantiate(splitsInto[i].gameObject, transform.position, Quaternion.identity);
            float ballDirection = i % 2 == 0 ? m_Rigidbody.velocity.x : -m_Rigidbody.velocity.x; 
            go.GetComponent<Rigidbody>().AddForce(new Vector3(ballDirection, 0,0), ForceMode.Impulse);

            if (m_Rigidbody.constraints == RigidbodyConstraints.FreezeAll)
                go.GetComponent<Ball>().Freeze();

            LevelManager.Instance.AddBall(go.GetComponent<Ball>());

        }
    }

    public void Freeze()
    {
        previousVelocity = m_Rigidbody.velocity;
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void UnFreeze()
    {
        m_Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
        m_Rigidbody.velocity = previousVelocity;
    }

    public void Rebound()
    {
        m_Rigidbody.velocity = -m_Rigidbody.velocity;
    }
}
