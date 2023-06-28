using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameLoop : MonoBehaviour
{
    private float timePassed = 0f;
    private float nextTimeStep;
    private float timeIncrement = 0.25f;
    private float scoreIncreaseFrequency = 1;
    private static string GAME_UI_PATH = $"GameUICanvas/GameUITable";
    private Transform scoreText;
    private Transform healthText;

    public GameObject gameScreenObject;

    public GameObject gameStateObject;
    private GameState gameState;

    public GameObject SnakeObject;
    private Snake snake;

    public GameObject deathScreenObject;
    private DeathScreen deathScreen;

    void Start()
    {
        snake = SnakeObject.GetComponent<Snake>();
        gameState = gameStateObject.GetComponent<GameState>();
        deathScreen = deathScreenObject.GetComponent<DeathScreen>();

        scoreText = gameScreenObject.transform.Find($"{GAME_UI_PATH}/CurrentScoreValueText");
        healthText = gameScreenObject.transform.Find($"{GAME_UI_PATH}/CurrentHPValueText");
        healthText.GetComponent<TextMeshProUGUI>().text = snake.startHP.ToString();
    }

    void Update()
    {
        if (gameState.currentState != GameState.State.Endless) return;

        if (snake.healthPoints == 0)
        {
            deathScreen.SubmitFinalScore(snake.score);
            gameState.SetStateDeath();
        }

        if (Time.time >= nextTimeStep)
        {
            nextTimeStep += timeIncrement;
            timePassed += timeIncrement;

            if (timePassed % scoreIncreaseFrequency == 0)
            {
                IncreaseScore();
            }
        }
    }

    public void IncreaseScore()
    {
        snake.score += snake.scoreIncrement;
        scoreText.GetComponent<TextMeshProUGUI>().text = snake.ScoreAsString;
    }

    public void TakeDamage(int damageTaken)
    {
        snake.healthPoints -= damageTaken;
        healthText.GetComponent<TextMeshProUGUI>().text = snake.healthPoints.ToString();
    }

    public void StartTimer()
    {
        nextTimeStep = Time.time;
    }
}
