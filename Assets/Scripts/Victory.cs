using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
    public Animator animator;

    private int sceneToLoad;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneFadeOut(3);
        }
    }

    public void SceneFadeOut(int levelIndex)
    {
        sceneToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
        StartCoroutine(ChangeToScene(sceneToLoad));
    }

    public IEnumerator ChangeToScene(int sceneToChangeTo)
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene (sceneToChangeTo);
    }
}
