using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float startingTime = 60f;
    public float timeLeft = 60f;

    public TMP_Text timeText, scoreText, finalScoreText;
    public GameObject endScreen, startScreen, scoreAudioPlay;

    bool gameStarted = false;
    float score = 0;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this; 
        else
        {
            Destroy(gameObject);
            return;
        }
        timeLeft = startingTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                EndGame();
            }
        }
        timeText.text = timeLeft.ToString("#.#") + "s";
        scoreText.text = "Score:\n" + score.ToString();

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameStarted = true;
            startScreen.SetActive(false);
        }

    }

    public static void AddScore(float x)
    {

        if (!Instance.gameStarted) return;
        Instance.score += x;
        Instance.scoreAudioPlay.GetComponent<AudioSource>().Play();
    }

    private void EndGame()
    {
        gameStarted = false;
        endScreen.SetActive(true);
        finalScoreText.text = "Your Score: " + score.ToString();
    }

}
