using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
        GetComponent<Animator>().SetTrigger("levelEnd");
        StartCoroutine(LevelEndDelay());
    }

    IEnumerator LevelEndDelay()
    {
        yield return new WaitForSeconds(levelLoadDelay);
        var currentScene = SceneManager.GetActiveScene().buildIndex;
        Destroy(FindObjectOfType<SceneSession>().gameObject);
        SceneManager.LoadScene(currentScene + 1);
    }
}
