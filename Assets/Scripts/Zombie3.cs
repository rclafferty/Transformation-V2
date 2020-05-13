using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie3 : MonoBehaviour
{
    enum State { Idle, Follow };
    State zombieState = State.Idle;

    [SerializeField] public GameObject player;
    NavMeshAgent navMeshAgent;
    Animator animator;

    [SerializeField] KeyManager keyManager;

    [SerializeField] float remainingDistance;
    [SerializeField] bool isFollowing;
    [SerializeField] public bool isDead;

    static int numberOfZombiesInScene = 0;
    static readonly float MAX_FOLLOW_DISTANCE = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        numberOfZombiesInScene++;
        
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        StartCoroutine(TrackTarget());
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowing)
        {
            navMeshAgent.SetDestination(new Vector3(player.transform.position.x, 0.05f, player.transform.position.z));
            remainingDistance = navMeshAgent.remainingDistance;

            if (remainingDistance > MAX_FOLLOW_DISTANCE && !float.IsInfinity(remainingDistance))
            {
                // Lost them
                isFollowing = false;
                navMeshAgent.isStopped = true;
                remainingDistance = -1;
                animator.SetFloat("Run", 0.0f);
            }
        }
    }

    // Tracking target
    IEnumerator TrackTarget()
    {
        while (!isDead)
        {
            if (zombieState == State.Idle)
            {
                // Look for target
                RaycastHit raycastHit;
                if (LookForTarget(out raycastHit))
                {
                    // Found the player
                    zombieState = State.Follow;

                    // Look at player
                    transform.LookAt(player.transform);

                    // Scream & Wait
                    animator.SetBool("isMoving", true);
                    animator.SetTrigger("Scream");
                    animator.SetFloat("Run", 2.0f);

                    navMeshAgent.speed = 4;

                    yield return new WaitForSeconds(2.5f);
                }
            }
            else // if follow state
            {
                // Follow target
                UpdateTargetLocation();
                
                if (navMeshAgent.remainingDistance < 3.0f)
                {
                    // Close enough
                    navMeshAgent.isStopped = true;

                    animator.SetBool("isMoving", false);
                    animator.SetFloat("Run", 0.0f);

                    animator.SetTrigger("AttackTrigger");
                    animator.SetBool("Attack", true);
                }
                else
                {
                    navMeshAgent.isStopped = false;
                    animator.SetBool("isMoving", true);
                    animator.SetFloat("Run", 2.0f);
                    animator.SetBool("Attack", false);
                }
            }

            remainingDistance = navMeshAgent.remainingDistance;
            yield return new WaitForSeconds(0.1f);
        }
    }

    bool LookForTarget(out RaycastHit raycastHit)
    {
        var rayDirection = player.transform.position - transform.position;
        if (Physics.Raycast(transform.position, rayDirection, out raycastHit))
        {
            if (raycastHit.transform == player.transform && raycastHit.distance < MAX_FOLLOW_DISTANCE)
            {
                return true;
            }
        }

        return false;
    }

    void UpdateTargetLocation()
    {
        Vector3 playerNavMeshLocation = new Vector3(player.transform.position.x, 0.05f, player.transform.position.z);
        if (false == navMeshAgent.SetDestination(playerNavMeshLocation))
        {
            // Error
            Debug.Log("Error in setting navigation");
            remainingDistance = -1;
        }
        else
        {
            // Found them
            remainingDistance = navMeshAgent.remainingDistance;
        }
    }
    
    private void Die()
    {
        isDead = true;
        isFollowing = false;
        navMeshAgent.SetDestination(transform.position);
        StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        numberOfZombiesInScene--;

        if (numberOfZombiesInScene > 0)
        {
            // Debug.Log(numberOfZombiesInScene + " left");
        }
        else
        {
            Debug.Log("All done!");
        }

        keyManager.KeyChance(transform);

        yield return new WaitForSeconds(10);

        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform == player.transform)
        {
            if (name.Contains("Zombie") && Input.GetMouseButtonDown(0) && !isDead)
            {
                // Destroy(gameObject);
                animator.SetTrigger("Dead");
                Die();
            }
        }
    }
}
