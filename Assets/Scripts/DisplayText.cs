using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DisplayText : MonoBehaviour
{
    [SerializeField] TextAsset text;
    [SerializeField] Text screenText;
    [SerializeField] Text continueText;

    [SerializeField] string nextSceneName;

    float TEXT_DELAY = 0.075f;

    // Start is called before the first frame update
    void Start()
    {
        continueText.gameObject.SetActive(false);
        StartCoroutine(DisplayTextToScreen());

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Stage 1
            SceneManager.LoadScene(nextSceneName);
        }
    }

    IEnumerator DisplayTextToScreen()
    {
        screenText.text = "";
        
        string[] lines = text.text.Split('\n');

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                TEXT_DELAY *= 3;
                Debug.Log(TEXT_DELAY);
                continue;
            }

            for (int i = 0; i < line.Length; i++)
            {
                screenText.text += line[i];

                yield return new WaitForSeconds(TEXT_DELAY);
            }

            screenText.text += "\n\n";
        }

        yield return new WaitForSeconds(2);
        continueText.gameObject.SetActive(true);
    }
}
