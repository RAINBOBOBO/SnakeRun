using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    private Scoreboard scoreboard;
    private bool isNewHighScore;
    private int finalScore;
    
    public GameObject scoreBoardObject;
    public GameObject newHighScoreObject;
    public GameObject scoreObject;
    
    void Start()
    {
        scoreboard = scoreBoardObject.GetComponent<Scoreboard>();
    }

    public void UpdateDeathScreen()
    {
        newHighScoreObject.SetActive(false);
        if (isNewHighScore) newHighScoreObject.SetActive(true);
        scoreObject.GetComponent<TextMeshProUGUI>().text = finalScore.ToString();
    }

    public void ResetDeathScreen()
    {
        newHighScoreObject.SetActive(false);
        isNewHighScore = false;
        finalScore = 0;
    }

    public void SubmitFinalScore(int score)
    {
        finalScore = score;
        isNewHighScore = scoreboard.SubmitScoreForHighScoreEntry(score);
    }


}
