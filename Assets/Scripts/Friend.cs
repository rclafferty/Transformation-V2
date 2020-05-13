using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Friend : MonoBehaviour
{
    [SerializeField] GameObject player;
    NavMeshAgent navMeshAgent;

    const float MAX_FOLLOW_DISTANCE = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFollowing)
        {
            navMeshAgent.SetDestination(new Vector3(player.transform.position.x, 0.05f, player.transform.position.z));

            if (navMeshAgent.remainingDistance < 4.0f)
            {
                // Close enough
                navMeshAgent.isStopped = true;
            }
            else if (navMeshAgent.remainingDistance > MAX_FOLLOW_DISTANCE && !float.IsInfinity(navMeshAgent.remainingDistance))
            {
                // Lost them
                IsFollowing = false;
                navMeshAgent.isStopped = true;
            }
            else
            {
                navMeshAgent.isStopped = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform == player.transform && Input.GetMouseButtonDown(0))
        {
            IsFollowing = true;
            navMeshAgent.isStopped = false;
        }
    }

    public bool IsFollowing { get; set; }
}
