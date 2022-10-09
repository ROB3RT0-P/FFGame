using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController controller;
    public AudioClip playerHitSound;
    AudioSource audioSource;
    Rigidbody rb;
    public Animator animator;
    public Animator animatorScene;
    private bool noHP = false;
    private int sceneToLoad;
    private float speed = 8.0f;
    public int maxHealth = 100;
    public int currentHealth { get; private set; }
    public float force = 10.0f;
    public Stat damage;
    float xMin = -4f;
    float xMax = 115f;
    float zMin = -11f;
    float zMax = 3f;

    //Push back
    float mass = 3.0f; // defines the character mass
    Vector3 impact = Vector3.zero;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
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

        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0,  Input.GetAxis("Vertical"));
        
        Vector3 pos = transform.position;
        
        /*
        TODO: Implemant boundary collision with math.clamp

        pos.x = Mathf.Clamp(pos.x + direction, -4, 115);
        pos.y = Mathf.Clamp(pos.z + direction, -11, 3);
        */

        //Check x axis & z axis boundaries        
        if(pos.x < xMin){
            pos.x = xMin;
            Debug.Log("xMin Boundary hit");
        }
        else if(pos.x > xMax){
            pos.x = xMax;
            Debug.Log("xMax Boundary hit");
        }
        else if(pos.z < zMin){
            pos.z = zMin;
            Debug.Log("zMin Boundary hit");
        }
        else if(pos.z > zMax){
            pos.z = zMax;
            Debug.Log("zMax Boundary hit");
        }

        //direction = pos;

        controller.Move(direction * Time.deltaTime * speed);

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Interactable");
            TakeDamage(1);
            ApplyPushBack();
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

    public void PushBack(Vector3 dir, float force)
    {
        dir.Normalize();
        if (dir.y < 0) 
        {
            dir.y = -dir.y;
        }
        impact += dir.normalized * force / mass;
    }

    public void ApplyPushBack()
    {
        if (impact.magnitude > 0.2) //Apply force
           {
                controller.Move(impact * Time.deltaTime);
           }
        impact = Vector3.Lerp(impact, Vector3.zero, 5*Time.deltaTime); //Negate force after
    }

    public virtual void NoHP()
    {
        Debug.Log("Player Died");
        noHP = true;
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
}