using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenZombieWaypoint : MonoBehaviour
{
    [SerializeField] GameObject zombiePrefab;
    [SerializeField] GameObject startingPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        // Destroy
        Destroy(other.gameObject);

        // Respawn
        GameObject g = Instantiate(zombiePrefab, startingPoint.transform.position, Quaternion.identity);
        g.GetComponent<Zombie3>().player = this.gameObject;
    }
}
