using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSession : MonoBehaviour
{
    int startSceneIndex;

    private void Awake()
    {
        var sessions = FindObjectsOfType<SceneSession>().Length;

        if (sessions > 1)
        {
            if (startSceneIndex != SceneManager.GetActiveScene().buildIndex)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
