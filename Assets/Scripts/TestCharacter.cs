using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class TestCharacter : Character
{
    public NavMeshAgent agent;
    public Vector3 tpos;

    public float searchRadius = 30f;
    public float searchDelay = 3f;
    public float searchTimer;

    public float maxDistance = 5f;

    void Start()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, maxDistance, NavMesh.AllAreas))
        {
            // Перемещаем объект на найденную точку
            transform.position = hit.position;
        }
        else
        {
            Debug.LogWarning("NavMesh point not found from " + transform.position);
            return;
        }
        searchTimer = searchDelay;
        
        StartCoroutine(TraverseLinks());
    }

    void FixedUpdate()
    {
        if (isRidden)
        {
            if (rider == null)
            {
                isRidden = false;
                return;
            }
            Vector3 pos = rider.transform.position;
            pos.x = transform.position.x;
            pos.z = transform.position.z;
            rider.transform.position = pos;
            rider.transform.localRotation = transform.localRotation;
            return;
        }
        searchTimer += Time.deltaTime;

        if (searchTimer >= searchDelay)
        {
            Vector3 newPos = RandomNavmeshLocation(searchRadius);
            agent.SetDestination(newPos);
            searchTimer = 0;
        }
    }
    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    IEnumerator TraverseLinks()
    {
        while (true)
        {
            if (agent.isOnOffMeshLink)
            {
                // Красиво двигаем агента вместо телепорта
                Vector3 startPos = agent.transform.position;
                Vector3 endPos = agent.currentOffMeshLinkData.endPos;
                
                float t = 0f;
                while (t < 1f)
                {
                    t += Time.deltaTime * 2f; // скорость подъема
                    agent.transform.position = Vector3.Lerp(startPos, endPos, t);
                    yield return null;
                }
                
                agent.CompleteOffMeshLink(); // сказать агенту "линк пройден"
            }
            yield return null;
        }
    }
}
