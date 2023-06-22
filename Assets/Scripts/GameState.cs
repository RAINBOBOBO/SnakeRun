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
    private State previousState;
    
    public GameObject snakeHeadControllerObject;
    private SnakeHeadController snakeHeadController;

    public GameObject snakeObject;
    private Snake snake;

    public GameObject scoreScreen;

    public GameObject menuScreen;


    void Start()
    {
        previousState = currentState;
        snakeHeadController = snakeHeadControllerObject.GetComponent<SnakeHeadController>();
        snake = snakeObject.GetComponent<Snake>();
    }

    void Update()
    {
        if (previousState != currentState && currentState == State.Menu) ShowMenu();

        if (previousState == State.Menu && currentState != State.Menu) HideMenu();
        
        if (previousState != currentState && currentState == State.EndlessLoading) StartEndless();

        if (previousState == State.Endless && currentState != State.Endless) EndEndless();

        if (previousState != currentState && currentState == State.Score) ShowScore();

        if (previousState == State.Score && currentState != State.Score) HideScore();

        if (previousState != currentState) previousState = currentState;
    }

    public void SetStateEndless() { currentState = State.EndlessLoading; }

    public void SetStateScore() { currentState = State.Score; }

    void ShowMenu()
    {
        menuScreen.SetActive(true);
    }

    void HideMenu()
    {
        menuScreen.SetActive(false);
    }
    
    public void ShowScore()
    {
        Debug.Log($"Final Score: {snake.score}");
        scoreScreen.SetActive(true);
    }

    void HideScore()
    {
        scoreScreen.SetActive(false);
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
