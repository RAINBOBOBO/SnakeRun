using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameState : MonoBehaviour
{
    public enum State
    {
        Menu, Endless, EndlessLoading, Tutorial, Settings, DevSettings, Score, Death
    }
    public State currentState;
    private State previousState;
    
    public GameObject snakeHeadControllerObject;
    private SnakeHeadController snakeHeadController;

    public GameObject snakeObject;
    private Snake snake;

    public GameObject menuScreen;
    public GameObject scoreScreen;
    public GameObject settingsScreen;
    public GameObject devSettingsScreen;
    public GameObject deathScreen;

    void Start()
    {
        previousState = currentState;
        snakeHeadController = snakeHeadControllerObject.GetComponent<SnakeHeadController>();
        snake = snakeObject.GetComponent<Snake>();
        ResetSceneAtStart();
    }

    void Update()
    {
        if (previousState != currentState && currentState == State.Menu) ShowMenu();
        if (previousState == State.Menu && currentState != State.Menu) HideMenu();
        
        if (previousState != currentState && currentState == State.EndlessLoading) StartEndless();
        if (previousState == State.Endless && currentState != State.Endless) EndEndless();

        if (previousState != currentState && currentState == State.Score) ShowScore();
        if (previousState == State.Score && currentState != State.Score) HideScore();

        if (previousState != currentState && currentState == State.Settings) ShowSettings();
        if (previousState == State.Settings && currentState != State.Settings) HideSettings();

        if (previousState != currentState && currentState == State.DevSettings) ShowDevSettings();
        if (previousState == State.DevSettings && currentState != State.DevSettings) HideDevSettings();

        if (previousState != currentState && currentState == State.Death) ShowDeath();
        if (previousState == State.Death && currentState != State.Death) HideDeath();

        if (previousState != currentState) previousState = currentState;
    }

    public void SetStateMenu() { currentState = State.Menu; }
    public void SetStateEndless() { currentState = State.EndlessLoading; }
    public void SetStateScore() { currentState = State.Score; }
    public void SetStateSettings() { currentState = State.Settings; }
    public void SetStateDevSettings() { currentState = State.DevSettings; }

    public void SetStateDeath() { currentState = State.Death; }

    void ResetSceneAtStart()
    {
        currentState = State.Menu;
        HideScore();
        HideSettings();
        HideDevSettings();
        HideDeath();
        ShowMenu();
    }
    
    void ShowMenu()
    {
        menuScreen.SetActive(true);
    }

    void HideMenu()
    {
        menuScreen.SetActive(false);
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
    
    public void ShowScore()
    {
        //Debug.Log($"Final Score: {snake.score}");
        scoreScreen.SetActive(true);
    }

    void HideScore()
    {
        scoreScreen.SetActive(false);
    }

    void ShowSettings()
    {
        settingsScreen.SetActive(true);
    }

    void HideSettings()
    {
        settingsScreen.SetActive(false);
    }

    void ShowDevSettings()
    {
        devSettingsScreen.SetActive(true);
    }

    void HideDevSettings()
    {
        devSettingsScreen.SetActive(false);
    }

    void ShowDeath()
    {
        deathScreen.SetActive(true);
    }

    void HideDeath()
    {
        deathScreen.SetActive(false);
    }
}
