using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TestEnemy : Character
{
    public Transform player;
    public float moveSpeed;
    public float attackRange;
    public float detectionRange;

    public NavMeshAgent agent;

    override protected void Update()
    {
        base.Update();
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (state)
        {
            case CharacterState.Idle:
                if (distanceToPlayer <= detectionRange)
                {
                    state = CharacterState.Attacking;
                }
                break;

            case CharacterState.Attacking:
                if (distanceToPlayer > detectionRange)
                {
                    state = CharacterState.Idle;
                }
                else if (distanceToPlayer <= attackRange)
                {
                    agent.SetDestination(transform.position); // Stop moving
                    Attack(player.GetComponent<Player>());
                }
                else
                {
                    // Move towards the player
                    agent.SetDestination(player.position);
                }
                break;
        }
    }
}
