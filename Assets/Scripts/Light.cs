using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    [SerializeField] bool isFlickering = true;
    Animator animator;
    WaitForSeconds randomOnDelay;
    WaitForSeconds randomDelay;
    WaitForSeconds randomDelay2;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        randomOnDelay = new WaitForSeconds(Random.Range(2, 5));
        randomDelay = new WaitForSeconds(Random.Range(3.0f, 30.0f));
        randomDelay2 = new WaitForSeconds(Random.Range(3.0f, 30.0f));

        if (isFlickering)
            StartCoroutine(Flicker());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Flicker()
    {
        yield return randomDelay;
        while (true)
        {
            animator.SetTrigger("Off");
            yield return randomOnDelay;

            animator.SetTrigger("On");

            yield return randomDelay;

            animator.SetTrigger("Off");
            yield return randomOnDelay;

            animator.SetTrigger("On");

            yield return randomDelay2;
        }
    }
}
