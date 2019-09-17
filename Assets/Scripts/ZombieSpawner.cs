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
        for (int i = 0; i < 3; i++)
            // StartCoroutine(SpawnZombies(20, xMaxBound:200, yMaxBound:200));
            StartCoroutine(SpawnZombies());
    }

    IEnumerator SpawnZombies(int numZombies = 5, float delay = 0.5f, float xMinBound = 6.0f, float xMaxBound = 20.0f, float yMinBound = 6.0f, float yMaxBound = 20.0f)
    {
        float thisX = 0.0f;
        float thisZ = 0.0f;

        NavMeshAgent nma;

        for (int i = 0; i < numZombies; i++)
        {
            thisX = Random.Range(xMinBound, xMaxBound);
            thisZ = Random.Range(yMinBound, yMaxBound);

            GameObject g = Instantiate(zombiePrefab, new Vector3(thisX, 0, thisZ), Quaternion.identity);
            nma = g.GetComponent<NavMeshAgent>();
            
            if (!nma.isOnNavMesh)
            {
                Destroy(g);
                i--;
                continue;
            }

            yield return new WaitForSeconds(delay);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
