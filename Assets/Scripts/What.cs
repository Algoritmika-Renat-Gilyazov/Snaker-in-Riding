using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class What : MonoBehaviour
{
    public int health = 10;
    public int maxHealth = 10;
    public int damage = 20;
    public NavMeshAgent agent;

    public Transform target;

    public void HitPlayer() {
        // player.TakeDamage(damage);
    }

    void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }

    public void TakeDamage()
    {
        health -= 1;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void TargetPlayer()
    {

    }
}