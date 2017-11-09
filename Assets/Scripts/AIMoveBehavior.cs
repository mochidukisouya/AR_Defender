using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIMoveBehavior : MonoBehaviour {
    public Animator animator;
    public NavMeshAgent navMeshAgent;
    public string animSpeedParam = "speed";
    public float turnSmooth = 15f;

    private void Awake()
    {
        navMeshAgent.updateRotation = false;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        float speed = navMeshAgent.desiredVelocity.sqrMagnitude;
        if (navMeshAgent.enabled) {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                speed = 0f;
                //navMeshAgent.isStopped = true;
            }
            else if (speed != 0f)
            {
                Quaternion lookRatation = Quaternion.LookRotation(navMeshAgent.desiredVelocity);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRatation, turnSmooth * Time.deltaTime);

            }
        }
        

        animator.SetFloat(animSpeedParam, speed);
        
    }
}
