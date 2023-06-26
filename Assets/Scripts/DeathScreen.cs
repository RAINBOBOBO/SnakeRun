using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    private Scoreboard scoreboard;
    private bool isNewHighScore;
    private int finalScore;
    
    public GameObject scoreBoardObject;
    public GameObject newHighScoreObject;
    public GameObject newLowScoreObject;
    
    void Start()
    {
        scoreboard = scoreBoardObject.GetComponent<Scoreboard>();
    }

    private void OnEnable()
    {
        newHighScoreObject.SetActive(false);
        if (isNewHighScore) newHighScoreObject.SetActive(true);

    }

    private void OnDisable()
    {
        newHighScoreObject.SetActive(false);
    }

    public void SubmitFinalScore(int score)
    {
        finalScore = score;
        isNewHighScore = scoreboard.SubmitScoreForHighScoreEntry(score);
    }


}
