using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
    NavMeshAgent nma;

    [SerializeField] bool isFollowing;

    // Start is called before the first frame update
    void Start()
    {
        nma = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowing)
        {
            nma.SetDestination(player.position);

            if (Vector3.Distance(transform.position, player.position) > 10.0f)
            {
                isFollowing = false;
                nma.isStopped = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isFollowing)
            return;

        if (other.name == "FPSController")
        {
            Debug.Log("Following");
            isFollowing = true;
        }
    }
}
