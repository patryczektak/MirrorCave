using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float startingTime = 60f;
    public float timeLeft = 60f;

    public TMP_Text timeText, scoreText;

    bool gameStarted = true;
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
        timeText.text = timeLeft.ToString("#.##") + "s";
        scoreText.text = "Score:\n" + score.ToString();
    }

    public static void AddScore(float x)
    {
        Instance.score += x;
    }

    private void EndGame()
    {
        gameStarted = false;
        Debug.LogWarning("END GAME MOCKUP");
    }

}
