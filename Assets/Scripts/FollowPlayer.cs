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
            Vector3 position = player.position;
            Debug.Log(position);
            nma.SetDestination(position);
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
            nma.isStopped = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (name.Contains("Zombie") && Input.GetMouseButtonDown(0))
        {
            Destroy(gameObject);
        }
    }
}
