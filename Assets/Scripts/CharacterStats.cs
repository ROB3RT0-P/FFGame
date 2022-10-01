using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterStats : MonoBehaviour
{
    public Animator animator;

    public Animator animatorScene;

    private int sceneToLoad;

    public int maxHealth = 100;

    public int currentHealth { get; private set; }

    public Stat damage;

    public Rigidbody rb;

    public AudioClip playerHitSound;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        //audioSource.PlayOneShot(playerHitSound, 0.5F);
        Debug.Log("Took damage: " + damage);

        if (currentHealth <= 0)
        {
            NoHP();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    public virtual void NoHP()
    {
        Debug.Log("Player Died");
        rb.constraints = RigidbodyConstraints.None;
        animator.SetTrigger("NoHP");
        StartCoroutine(waiter());
        SceneFadeOut(2);
    }

    IEnumerator waiter()
    {
        //Wait for 4 seconds
        yield return new WaitForSeconds(4);
    }

    public void SceneFadeOut(int levelIndex)
    {
        sceneToLoad = levelIndex;
        StartCoroutine(ChangeToScene(sceneToLoad));
    }

    public IEnumerator ChangeToScene(int sceneToChangeTo)
    {
        yield return new WaitForSeconds(4);
        animatorScene.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene (sceneToChangeTo);
    }
}
