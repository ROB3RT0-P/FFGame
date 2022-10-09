using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMovement : MonoBehaviour
{
    public Animator animatorScene;
    public Animator animator;
    public AudioClip playerHitSound;
    AudioSource audioSource;
    Rigidbody rb;
    private bool noHP = false;
    private int sceneToLoad;
    public int maxHealth = 100;
    public int currentHealth { get; private set; }
    public Stat damage;
    private float speed = 20.0f;
    private float force = 40.0f;
    float xMin = -4f;
    float xMax = 115f;
    float zMin = -11f;
    float zMax = 3f;
   


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>(); 
        rb.isKinematic = true;
    }

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void FixedUpdate()
    {
        if (!noHP)
        {
           Movement(); 
        }

        //for debugging
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(1);
        }
    }
    
    void Movement()
    {
        animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        animator.SetFloat("Vertical", Input.GetAxis("Vertical"));

        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //Check x axis & z axis boundaries        
        if(transform.position.x < xMin){
            transform.position = new Vector3(xMin, transform.position.y, transform.position.z);
            Debug.Log("xMin Boundary hit");
        }
        else if(transform.position.x > xMax){
            transform.position = new Vector3(xMax, transform.position.y, transform.position.z);
            Debug.Log("xMax Boundary hit");
        }
        else if(transform.position.z < zMin){
            transform.position = new Vector3(transform.position.x, transform.position.y, zMin);
            Debug.Log("zMin Boundary hit");
        }
        else if(transform.position.z > zMax){
            transform.position = new Vector3(transform.position.x, transform.position.y, zMax);
            Debug.Log("zMax Boundary hit");
        }
        
        transform.position += direction * Time.deltaTime * speed;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Debug.Log("Interactable");
            TakeDamage(1);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Not Interactable");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        audioSource.PlayOneShot(playerHitSound, 0.5F); //not working
        Debug.Log("Took damage: " + damage);

        if (currentHealth <= 0)
        {
            NoHP();
        }
    }

    public virtual void NoHP()
    {
        Debug.Log("Player Died");
        noHP = true;
        rb.constraints = RigidbodyConstraints.FreezePosition;
        rb.isKinematic = true;//not working
        rb.detectCollisions = false;//not working
        animator.SetTrigger("NoHP");
        SceneFadeOut(2);
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
    /*
    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Enemy")
        {
            Vector3 dir = c.contacts[0].point - transform.position;
            dir = -dir.normalized;
            GetComponent<Rigidbody>().AddForce(dir * force);
        }
    } 
    */   
}