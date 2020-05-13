using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackBox : MonoBehaviour
{
    [SerializeField] Animator parentAnimator;
    [SerializeField] Player player;
    [SerializeField] Zombie3 parentZombie;

    bool hit = false;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").GetComponent<Player>();
        }

        StartCoroutine(HitRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (parentZombie != null)
        {
            if (parentZombie.isDead)
            {
                StopAllCoroutines();
                this.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            hit = true;
        }
    }

    IEnumerator HitRoutine()
    {
        while (true)
        {
            if (hit)
            {
                hit = false;
                player.Hit(transform.parent);
                yield return new WaitForSeconds(2);
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
