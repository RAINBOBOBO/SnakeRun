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
    private static string GAME_UI_PATH = $"GameUICanvas";
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

        scoreText = gameScreenObject.transform.Find($"{GAME_UI_PATH}/ScoreValueText");
        healthText = gameScreenObject.transform.Find($"{GAME_UI_PATH}/HealthValueText");
        healthText.GetComponent<TextMeshProUGUI>().text = snake.startHP.ToString();
    }

    void Update()
    {
        if (gameState.currentState != GameState.State.Endless) return;

        if (snake.healthPoints == 0)
        {
            GameOver();
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

    public void GameOver()
    {
        deathScreen.SubmitFinalScore(snake.score);
        gameState.SetStateDeath();
    }

    public List<BoxCollider2D> GetBorderBounds()
    {
        List<BoxCollider2D> bounds = new List<BoxCollider2D>();
        BoxCollider2D borderNorth = transform.Find($"{GAME_UI_PATH}/BorderNorth").GetComponent<BoxCollider2D>();
        BoxCollider2D borderSouth = transform.Find($"{GAME_UI_PATH}/BorderSouth").GetComponent<BoxCollider2D>();
        BoxCollider2D borderEast = transform.Find($"{GAME_UI_PATH}/BorderEast").GetComponent<BoxCollider2D>();
        BoxCollider2D borderWest = transform.Find($"{GAME_UI_PATH}/BorderWest").GetComponent<BoxCollider2D>();

        bounds.Add(borderNorth);
        bounds.Add(borderSouth);
        bounds.Add(borderEast);
        bounds.Add(borderWest);

        return bounds;
    }
}
