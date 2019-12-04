using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int lives = 3;
    [SerializeField] int score = 0;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI livesText;

    private void Awake()
    {
        var sessions = FindObjectsOfType<GameSession>().Length;

        if (sessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
    }

    public void ProcessPlayerDeath()
    {
        if (lives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    public void AddToScore(int points)
    {
        score += points;
        UpdateText();
    }

    private void TakeLife()
    {
        lives--;
        UpdateText();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(FindObjectOfType<SceneSession>().gameObject);
        Destroy(gameObject);
    }

    private void UpdateText()
    {
        livesText.text = lives.ToString();
        scoreText.text = score.ToString();
    }
}
