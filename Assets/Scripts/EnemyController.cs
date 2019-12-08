using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    public GameObject target;
    private NavMeshAgent agent;
    private enum States { wandering, followingTarget };
    private States state = States.wandering;

    public float wanderRadius = 20;
    private float wanderTimer = 3;
    private float followTimer = 1;
    private float timer = 0;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        float dist = DistanceToTarget();
        if (dist > wanderRadius)
        {
            wander();
        } else
        {
           followTarget();
        }

        Debug.Log(this.state);

    }

    void followTarget()
    {
        if (timer >= followTimer)
        {
            this.state = States.followingTarget;
            Vector3 dest = target.transform.position;
            agent.SetDestination(dest);
            agent.speed = 5;
            timer = 0;
        }
    }

    void wander()
    {
        
        if (timer >= wanderTimer)
        {
            this.state = States.wandering;
            Vector3 newPos = RandomNavSphere(this.transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }

    }

    private float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, target.transform.position);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

}
