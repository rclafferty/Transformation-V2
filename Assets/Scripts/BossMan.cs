using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossMan : MonoBehaviour
{
    GameObject player;
    NavMeshAgent navMeshAgent;
    Animator animator;

    [SerializeField] Image healthBar;

    const int MAX_HEALTH = 100;
    int health = MAX_HEALTH;

    bool isFollowing = false;
    bool fightHasStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        healthBar.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        if (!fightHasStarted)
            return;

        if (Vector3.Distance(player.transform.position, transform.position) < 3.0f)
        {
            isFollowing = false;
            navMeshAgent.isStopped = true;

            // Attack!
            animator.SetTrigger("Punch");
        }
        else
        {
            Walk();
        }

        if (isFollowing)
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
    }

    public void Walk()
    {
        isFollowing = true;
        fightHasStarted = true;
        navMeshAgent.isStopped = false;

        animator.SetBool("Walking", true);
    }

    public void Hit()
    {
        if (health == 0)
            return;

        health -= 1;
        float x = (float)health / MAX_HEALTH;
        healthBar.rectTransform.localScale = new Vector3(x, 1, 1);

        Debug.Log("Health is now " + health);

        if (health == 0)
        {
            StartCoroutine(DeathRoutine());
        }
    }

    IEnumerator DeathRoutine()
    {
        GetComponent<Animator>().SetTrigger("Die");

        isFollowing = false;
        navMeshAgent.isStopped = true;

        yield return new WaitForSeconds(3);

        if (player.GetComponent<Player>().friendIsFollowing)
        {
            SceneManager.LoadScene("goodEnding");
        }
        else
        {
            SceneManager.LoadScene("badEnding");
        }
    }
}
