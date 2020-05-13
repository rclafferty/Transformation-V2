using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    [SerializeField] GameObject keyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KeyChance(Transform t)
    {
        if (!IsKeyDropped)
        {
            // Chance to drop the key
            if (Random.Range(0, 10) == 5)
            {
                Debug.Log("Dropping the key!");

                // Drop key
                GameObject g = Instantiate(keyPrefab, t.position, Quaternion.identity);
                g.name = "Key";
                Vector3 newPosition = g.transform.position;
                g.transform.position = new Vector3(newPosition.x, 1, newPosition.z);

                IsKeyDropped = true;
            }
        }
    }

    public bool IsKeyDropped { get; private set; }
}
