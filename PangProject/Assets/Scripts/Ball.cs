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
        m_Rigidbody.AddForce(initialDirection * initialForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Projectile")) return;

        Split();

        Destroy(collision.collider.gameObject);
        Destroy(this.gameObject);
    }

    private void Split()
    {
        for(int i = 0; i < splitsInto.Count; i++)
        {
            GameObject go = Instantiate(splitsInto[i].gameObject, transform.position, Quaternion.identity);
            Vector3 ballDirection = i % 2 == 0 ? Vector3.right : Vector3.left; 
            go.GetComponent<Rigidbody>().AddForce(ballDirection, ForceMode.Impulse);
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
}
