using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public enum State
    {
        Settings, ResetScoreboardConfirmation
    }
    public State currentState = State.Settings;
    private State previousState;

    public GameObject settingsTable;
    public GameObject resetHighScoresConfirmationTable;

    void Start()
    {
        previousState = currentState;
    }

    void Update()
    {
        if (previousState != currentState && currentState == State.Settings) ShowSettings();
        if (previousState == State.Settings && currentState != State.Settings) HideSettings();

        if (previousState != currentState && currentState == State.ResetScoreboardConfirmation) ShowResetScoreboardConfirmation();
        if (previousState == State.ResetScoreboardConfirmation && currentState != State.ResetScoreboardConfirmation) HideResetScoreboardConfirmation();

        if (previousState != currentState) previousState = currentState;
    }

    public void SetStateSettings() { currentState = State.Settings; }
    public void SetStateResetScoreboardConfirmation() { currentState = State.ResetScoreboardConfirmation; }

    void ShowSettings()
    {
        settingsTable.SetActive(true);
    }

    void HideSettings()
    {
        settingsTable.SetActive(false);
    }

    void ShowResetScoreboardConfirmation()
    {
        resetHighScoresConfirmationTable.SetActive(true);
    }

    void HideResetScoreboardConfirmation()
    {
        resetHighScoresConfirmationTable.SetActive(false);
    }
}
