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

    public GameObject scoreboardObject;
    private Scoreboard scoreboard;

    public GameObject deathScreenObject;
    private DeathScreen deathScreen;

    public GameObject gameLoopObject;
    private GameLoop gameLoop;

    public GameObject menuCanvas;
    public GameObject scoreCanvas;
    public GameObject deathCanvas;
    public GameObject settingsCanvas;
    public GameObject devSettingsCanvas;
    public GameObject gameUICanvas;

    void Start()
    {
        scoreboard = scoreboardObject.GetComponent<Scoreboard>();
        deathScreen = deathScreenObject.GetComponent<DeathScreen>();
        gameLoop = gameLoopObject.GetComponent<GameLoop>();

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
        HideGameUI();
        ShowMenu();
    }
    
    void ShowMenu()
    {
        menuCanvas.SetActive(true);
    }

    void HideMenu()
    {
        menuCanvas.SetActive(false);
    }

    void StartEndless()
    {
        snakeHeadController.ResetHead();
        snake.CreateSnake();
        snake.score = 0;
        ShowGameUI();
        gameLoop.StartTimer();
        currentState = State.Endless;
    }

    void EndEndless()
    {
        HideGameUI();
        snake.DeleteSnake();
        snakeHeadController.DeactivateHead();
    }
    
    public void ShowScore()
    {
        //Debug.Log($"Final Score: {snake.score}");
        scoreboard.ShowEntries();
        scoreCanvas.SetActive(true);

    }

    void HideScore()
    {
        scoreboard.HideEntries();
        scoreCanvas.SetActive(false);
    }

    void ShowSettings()
    {
        settingsCanvas.SetActive(true);
    }

    void HideSettings()
    {
        settingsCanvas.SetActive(false);
    }

    void ShowDevSettings()
    {
        devSettingsCanvas.SetActive(true);
    }

    void HideDevSettings()
    {
        devSettingsCanvas.SetActive(false);
    }

    void ShowDeath()
    {
        deathCanvas.SetActive(true);
        deathScreen.UpdateDeathScreen();
    }

    void HideDeath()
    {
        deathScreen.ResetDeathScreen();
        deathCanvas.SetActive(false);
    }

    void ShowGameUI()
    {
        gameUICanvas.SetActive(true);
    }

    void HideGameUI()
    {
        gameUICanvas.SetActive(false);
    }
}
