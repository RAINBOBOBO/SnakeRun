using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameState : MonoBehaviour
{
    public enum State
    {
        Menu, Endless, EndlessLoading, Tutorial, Settings, Score
    }
    public State currentState;
    public GameObject snakeHeadControllerObject;
    public GameObject snakeObject;

    private SnakeHeadController snakeHeadController;
    private Snake snake;
    private State previousState;


    void Start()
    {
        previousState = currentState;
        snakeHeadController = snakeHeadControllerObject.GetComponent<SnakeHeadController>();
        snake = snakeObject.GetComponent<Snake>();
    }

    void Update()
    {
        if (previousState != currentState && currentState == State.EndlessLoading)
        {
            StartEndless();
        }

        if (previousState == State.Endless && currentState != State.Endless)
        {
            EndEndless();
        }

        if (previousState != currentState) previousState = currentState;
    }

    void ShowMenu()
    {

    }
    
    public void ShowScore()
    {
        currentState = State.Score;
        Debug.Log($"Final Score: {snake.score}");
    }

    void StartEndless()
    {
        snakeHeadController.ResetHead();
        snake.CreateSnake();
        snake.score = 0;
        currentState = State.Endless;
    }

    void EndEndless()
    {
        snake.DeleteSnake();
        snakeHeadController.DeactivateHead();
    }
}
