using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMovement : MonoBehaviour
{
    private CharacterController controller;

    private bool groundedPlayer;

    public Animator animator;

    private float speed = 8.0f;

    private float gravityValue = -9.81f;

    private Vector3 playerVelocity;

    [SerializeField]
    private bool triggerActive = false;

    public float pushingForce = 10.0f;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            triggerActive = true;
            Debug.Log("Interactable");

            /*
                for(int i = 0; i < 1; ++i)
                {
                var magnitude = 500;
                var force = transform.position - other.transform.position;
                force.Normalize();
                gameObject.GetComponent<Rigidbody>().AddForce(force * magnitude);
                }
                */
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            triggerActive = false;
            Debug.Log("Not Interactable");
        }
    }

    void OnCollisionEnter(Collision c)
    {
        float force = 3;

        if (c.gameObject.tag == "Enemy")
        {
            Vector3 dir = c.contacts[0].point - transform.position;
            dir = -dir.normalized;
            GetComponent<Rigidbody>().AddForce(dir * force);
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
    }

    /*
    void Magic()
    {
  
        //destroy fire after 5 seconds
        Destroy(gameObject, 5);
    }
    */
    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        animator.SetFloat("Vertical", Input.GetAxis("Vertical"));

        Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        transform.position =
            transform.position + horizontal * Time.deltaTime * speed;

        Vector3 vertical = new Vector3(0, 0, Input.GetAxis("Vertical"));
        transform.position =
            transform.position + vertical * Time.deltaTime * speed;

        playerVelocity.y += gravityValue * Time.deltaTime;
        //controller.Move(playerVelocity * Time.deltaTime);

        /*
        if (triggerActive && Input.GetKeyDown(KeyCode.Space))
            {
               // Attack();
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
               // Attack();
            }
        */
    }
}
