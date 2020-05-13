using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour
{
    [SerializeField] FirstPersonController firstPersonController;
    [SerializeField] GameObject bossMan;

    [SerializeField] RawImage keyUIImage;
    [SerializeField] bool hasKey;

    [SerializeField] Camera FPControllerCamera;
    [SerializeField] Camera bossCamera;

    [SerializeField] Transform bossWaypoint;

    [SerializeField] Text friendFollowingText;
    [SerializeField] Image healthBar;

    bool isDead = false;
    [SerializeField] GameObject deathCanvas;
    [SerializeField] GameObject hud;

    int health = 6;

    Friend friend;
    public bool friendIsFollowing;

    // Start is called before the first frame update
    void Start()
    {
        friend = GameObject.Find("Friend").GetComponent<Friend>();
        firstPersonController = GetComponent<FirstPersonController>();
        hasKey = false;
        keyUIImage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Display UI
        if (friend.IsFollowing != friendIsFollowing)
        {
            UpdateFollowingUI(friend.IsFollowing);
        }
    }

    void UpdateFollowingUI(bool isFollowing)
    {
        friendIsFollowing = isFollowing;

        if (isFollowing)
        {
            friendFollowingText.text = "Friend: Found";
        }
        else
        {
            friendFollowingText.text = "Friend: Lost";
        }
    }

    public void ObtainKey()
    {
        keyUIImage.gameObject.SetActive(true);
        hasKey = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (isDead)
            return;

        if (other.name == "BossManDoor")
        {
            if (Input.GetMouseButtonDown(0) && hasKey)
            {
                Destroy(other.gameObject);

                // Destroy all zombies
                Zombie3[] zombies = GameObject.FindObjectsOfType<Zombie3>();

                foreach (Zombie3 z in zombies)
                {
                    Destroy(z.gameObject);
                }
            }
        }
        else if (other.name == "Key")
        {
            ObtainKey();
            Destroy(other.gameObject);
        }
        else if (other.name == "Boss Trigger")
        {
            other.gameObject.SetActive(false);
            StartCoroutine(BossCutscene());
        }
        else if (other.name == "BossMan" && Input.GetMouseButtonDown(0))
        {
            bossMan.GetComponent<BossMan>().Hit();
        }
    }

    IEnumerator BossCutscene()
    {
        firstPersonController.enabled = false;

        bossCamera.transform.position = FPControllerCamera.transform.position;

        bossCamera.enabled = true;
        FPControllerCamera.enabled = false;

        const float TRANSITION_TIME = 2.0f;

        Vector3 bossCameraInitialPosition = bossCamera.transform.position;
        Vector3 bossCameraEndPosition = bossWaypoint.transform.position;

        // Zoom to boss
        for (float time = 0; time < TRANSITION_TIME; time += Time.deltaTime)
        {
            float newX = Mathf.Lerp(bossCameraInitialPosition.x, bossCameraEndPosition.x, time / TRANSITION_TIME);

            float newZ = Mathf.Lerp(bossCameraInitialPosition.z, bossCameraEndPosition.z, time / TRANSITION_TIME);

            bossCamera.transform.position = new Vector3(newX, bossCameraInitialPosition.y, newZ);

            yield return new WaitForEndOfFrame();
        }

        // Play animation
        bossMan.GetComponent<Animator>().SetTrigger("Fight");
        yield return new WaitForSeconds(2);
        bossMan.GetComponent<BossMan>().Walk();

        // Zoom back
        for (float time = 0; time < TRANSITION_TIME; time += Time.deltaTime)
        {
            float newX = Mathf.Lerp(bossCameraEndPosition.x, bossCameraInitialPosition.x, time / TRANSITION_TIME);

            float newZ = Mathf.Lerp(bossCameraEndPosition.z, bossCameraInitialPosition.z, time / TRANSITION_TIME);

            bossCamera.transform.position = new Vector3(newX, bossCameraInitialPosition.y, newZ);

            yield return new WaitForEndOfFrame();
        }

        FPControllerCamera.enabled = true;
        bossCamera.enabled = false;

        firstPersonController.enabled = true;
    }

    public void Hit(Transform t)
    {
        if (isDead)
            return;

        StartCoroutine(Knockback(t));
    }

    IEnumerator Knockback(Transform t)
    {
        if (health > 0)
        {
            health--;
        }

        float scale = 0.0f;

        // If UPDATED health > 0
        if (health > 0)
        {
            scale = (float)health / 6.0f;
            healthBar.rectTransform.localScale = new Vector2(scale, 1);
        }
        else
        {
            // Dead
            isDead = true;

            scale = 0.05f;
            healthBar.color = Color.red;

            firstPersonController.enabled = false;
            deathCanvas.SetActive(true);
            hud.SetActive(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        healthBar.rectTransform.localScale = new Vector2(scale, 1);
        
        Debug.Log("Health: " + health);

        // firstPersonController.enabled = false;
        Vector3 direction = t.position - this.transform.position;
        this.GetComponent<Rigidbody>().AddExplosionForce((direction * 10).magnitude, t.position, 1.0f);

        yield return new WaitForSeconds(0.1f);
        // firstPersonController.enabled = true;
    }
}
