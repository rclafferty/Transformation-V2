using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Animator animator;
    bool moving;

    [SerializeField] bool isFollowingPlayer;
    [SerializeField] Transform player;

    // Start is called before the first frame update
    void Start()
    {
        moving = false;

        float scale = 1 / 3.1f;
        navMeshAgent.speed = scale;
        animator.speed = scale;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowingPlayer)
        {
            if (HasReachedDestination())
            {
                // Attack
                Debug.Log("Attack");
            }
            else
            {
                // Debug.Log(player.position);
                navMeshAgent.SetDestination(player.position);
            }
        }
        else if (moving)
        {
            if (HasReachedDestination())
            {
                Stop();
            }
        }
    }

    bool HasReachedDestination()
    {
        if (Vector3.Distance(navMeshAgent.destination, transform.position) < 0.4f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Stop()
    {
        Debug.Log("Reached destination");
        StartCoroutine(FadeMovement(0.0f, isMoving: false, setNewDestination: true));
    }

    void Walk(Vector3 destination)
    {
        navMeshAgent.SetDestination(destination);
        StartCoroutine(FadeMovement(1.0f, isMoving: true));
    }

    static float X_MIN = 6.0f;
    static float X_MAX = 100.0f;

    static float Z_MIN = 6.0f;
    static float Z_MAX = 100.0f;

    public void SetRandomDestination()
    {
        float thisX = Random.Range(X_MIN, X_MAX);
        float thisZ = Random.Range(Z_MIN, Z_MAX);

        // Debug.Log("X: " + thisX + " Z: " + thisZ);
        Walk(new Vector3(thisX, 0, thisZ));
    }

    IEnumerator FadeMovement(float end, bool isMoving, bool setNewDestination = false)
    {
        float time = 0.0f;
        float timeToCompletion = 1.0f;
        float value = animator.GetFloat("Run");
        float initValue = value;
        while (Mathf.Abs(value - end) >= 0.01f)
        {
            value = Mathf.Lerp(initValue, end, time / timeToCompletion);
            animator.SetFloat("Run", value);

            time += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        moving = isMoving;
        navMeshAgent.isStopped = !isMoving;
        animator.SetBool("isMoving", isMoving);

        if (setNewDestination)
        {
            SetRandomDestination();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Collided with " + other.name);

        if (other.name == "FPSController")
        {
            if (isFollowingPlayer)
                return;

            Debug.Log("Following player");
            player = other.transform;
            isFollowingPlayer = true;
        }
    }
}
