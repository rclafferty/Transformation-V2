using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceoverManager : MonoBehaviour
{
    [SerializeField] Image fadeImage;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Fade());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(7);

        Color newColor = fadeImage.color;

        const float TRANSITION_TIME = 2.5f;
        for (float time = 0; time < TRANSITION_TIME; time += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(1, 0, time / TRANSITION_TIME);
            newColor.a = alpha;
            fadeImage.color = newColor;

            yield return new WaitForEndOfFrame();
        }

        Destroy(fadeImage.gameObject);
    }
}
