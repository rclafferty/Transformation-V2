using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] GameObject zombiePrefab;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 1; i++)
            StartCoroutine(SpawnZombies());
    }

    IEnumerator SpawnZombies(float delay = 0.5f)
    {
        float thisX = 0.0f;
        float thisZ = 0.0f;

        NavMeshAgent nma;

        for (int i = 0; i < 2; i++)
        {
            thisX = Random.Range(6, 20);
            thisZ = Random.Range(6, 20);

            GameObject g = Instantiate(zombiePrefab, new Vector3(thisX, 0, thisZ), Quaternion.identity);
            nma = g.GetComponent<NavMeshAgent>();
            
            if (!nma.isOnNavMesh)
            {
                Destroy(g);
                i--;
                continue;
            }


            g.GetComponent<Zombie>().SetRandomDestination();

            yield return new WaitForSeconds(delay);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
