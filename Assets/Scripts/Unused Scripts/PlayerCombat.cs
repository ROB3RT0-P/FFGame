/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class PlayerCombat : MonoBehaviour
{

    [SerializeField] private bool triggerActive = false;
 
    public Animator animator;
    bool attacking = false;
    public int playerHealth = 100;
    public int enemyHealth = 25;


        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                triggerActive = true;
                Debug.Log("Interactable");
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
 
        private void Update()
        {    
            var rand = new System.Random();
            
            if(attacking == false){
            if (triggerActive && Input.GetKeyDown(KeyCode.Space))
            {
                attacking = true;
                Attack();
                enemyHealth = enemyHealth - rand.Next(1, 5);
                StartCoroutine(waiter());
                attacking = false;
                Debug.Log("Enemy HP:" + enemyHealth)
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
            }
            }

            if(playerHealth == 0)
            {
                Defeated();
            }
        }
 
        void Attack()
        {
            animator.SetTrigger("Attack");
        }

        void Defeated()
        {
            //animator.SetTrigger("Defeated");
            //SceneManager.LoadScene(GameOver);
        }

        public void OnEnemyCollide(Collider other)
        {
            if (other.CompareTag("Player"))
                {
                triggerActive = true;
                Debug.Log("Enemy Hit Player");
                //TODO: push back player
                }
        }
        
        IEnumerator waiter()
        {
            yield return new WaitForSeconds(1);
        }

        /* public void OnWallCollideNorth(Collider other)
        {
            if (other.CompareTag("NBoundary"))
            {
                triggerActive = true;
                Debug.Log("Hit Boundary");
                //TODO: push back player
            }
        }
        

        
 
}*/