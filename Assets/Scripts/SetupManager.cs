using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;

        RenderSettings.ambientEquatorColor = Color.black;
        RenderSettings.ambientGroundColor = Color.black;
        RenderSettings.ambientSkyColor = Color.black;
        RenderSettings.ambientLight = Color.black;
        RenderSettings.ambientIntensity = 0.0f;
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
