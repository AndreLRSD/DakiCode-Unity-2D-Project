using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gamecontrol : MonoBehaviour
{
    public Text healthtext;

    public int score;
    public Text scoreText;

    public int totalScore;

    public static Gamecontrol instance;
    public GameObject pauseObj;

    public GameObject gameOverObj;

    private bool isPaused;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        totalScore = PlayerPrefs.GetInt("score");
    }

    // Update is called once per frame
    void Update()
    {
        PauseGame();
    }

    public void UpdateScore(int value)
    {
        score += value;
        scoreText.text = score.ToString();

        PlayerPrefs.SetInt("score", score + totalScore);
    }

    public void Updatelives(int value)
    {
        healthtext.text = "x " + value.ToString();
    }

    public void PauseGame()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;
            pauseObj.SetActive(isPaused);
        }

        if(isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void GameOver()
    {
        gameOverObj.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }
}
