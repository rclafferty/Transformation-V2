using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie2 : MonoBehaviour
{
    [SerializeField] ZombieFollowPlayer followScript;
    [SerializeField] bool isFollowing;

    static int numberOfZombiesInScene = 0;

    // Start is called before the first frame update
    void Start()
    {
        numberOfZombiesInScene++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        numberOfZombiesInScene--;

        if (numberOfZombiesInScene > 0)
            Debug.Log(numberOfZombiesInScene + " left");
        else
            Debug.Log("All done!");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "FPSController")
        {
            if (name.Contains("Zombie") && Input.GetMouseButtonDown(0))
            {
                Destroy(gameObject);
            }
        }
    }
}
