using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieFollowPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
    NavMeshAgent navMeshAgent;

    [SerializeField] bool isFollowing;

    Vector3 playerPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            player = GameObject.Find("FPSController").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowing)
        {
            playerPosition = player.position;
            // Debug.Log(playerPosition);
            navMeshAgent.SetDestination(playerPosition);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "FPSController")
        {
            Debug.Log("Following");
            isFollowing = true;
            navMeshAgent.isStopped = false;
        }
    }
}
