using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float startingTime = 60f;
    float timeLeft = 60f;

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
        }
        timeText.text = timeLeft.ToString() + "s";
        timeText.text = "score: " + score.ToString();
    }

    public static void AddScore(float x)
    {
        Instance.score += x;
    }

}
