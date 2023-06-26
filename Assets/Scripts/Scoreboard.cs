using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    private Transform scoreContainer;
    private Transform entryTemplate;
    private List<HighScoreEntry> highScores = new List<HighScoreEntry>();
    private List<Transform> highScoreTransforms = new List<Transform>();

    public float templateHeight = 20f;

    [System.Serializable]
    private class HighScoreEntry
    {
        public int score;
        public string date;
        public bool isNewEntry;
    }

    private class HighScoreList
    {
        public List<HighScoreEntry> highScores;
    }

    private void Awake()
    {
        scoreContainer = transform.Find("ScoreContainer");
        entryTemplate = transform.Find("ScoreContainer/EntryTemplate");

        entryTemplate.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        HighScoreList highScoresToLoad = LoadHighScores();
        if (highScoresToLoad != null)
        {
            highScores = highScoresToLoad.highScores;

            foreach (HighScoreEntry entry in highScores)
            {
                CreateHighScoreEntryTransform(entry, scoreContainer, highScoreTransforms);
            }
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < highScores.Count; i++)
        {
            Transform currentTransform = transform.Find($"ScoreContainer/Entry {i + 1}");
            Destroy(currentTransform.gameObject);

            highScores[i].isNewEntry = false;
        }
        highScoreTransforms = new List<Transform>();
        SaveHighScores(highScores);
    }

    private void CreateHighScoreEntryTransform(HighScoreEntry entry, Transform container, List<Transform> transforms)
    {
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transforms.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transforms.Count + 1;
        entryTransform.gameObject.name = $"Entry {rank}";


        TextMeshProUGUI tmpPosition = entryTransform.Find("Position").GetComponent<TextMeshProUGUI>();
        tmpPosition.text = rank + ".";

        
        TextMeshProUGUI tmpDate = entryTransform.Find("Date").GetComponent<TextMeshProUGUI>();
        tmpDate.text = entry.date;

        TextMeshProUGUI tmpScore = entryTransform.Find("Score").GetComponent<TextMeshProUGUI>();
        tmpScore.text = entry.score.ToString("#,#");

        if (entry.isNewEntry)
        {
            Color newEntryColor = new Color(0.63f, 0.62f, 0.34f);
            tmpPosition.color = newEntryColor;
            tmpDate.color = newEntryColor;
            tmpScore.color = newEntryColor;
        }

        transforms.Add(entryTransform);
    }

    private bool InsertHighScoreIntoListSorted(HighScoreEntry entry, List<HighScoreEntry> scores)
    {
        entry.isNewEntry = true;
        if (scores.Count == 0)
        {
            scores.Add(entry);
            SaveHighScores(scores);
            return true;
        }

        for (int i = 0; i < scores.Count; i++)
        {
            if (scores[i].score < entry.score)
            {
                scores.Insert(i, entry);
                if (scores.Count > 10) scores.RemoveAt(scores.Count - 1);
                SaveHighScores(scores);
                return true;
            }

            if (scores[i].score == entry.score)
            {
                scores.Insert(i, entry);
                if (scores.Count > 10) scores.RemoveAt(scores.Count - 1);
                SaveHighScores(scores);
                return true;
            }
        }

        if (scores.Count < 10) 
        {
            scores.Insert(scores.Count, entry);
            SaveHighScores(scores);
            return true;
        }

        return false;
    }

    public bool SubmitScoreForHighScoreEntry(int score)
    {
        string date = DateTime.Now.ToString("M/d/yy");
        HighScoreEntry entry = new HighScoreEntry { date = date, score = score };
        bool isNewHighScore = InsertHighScoreIntoListSorted(entry, highScores);
        return isNewHighScore;
    }

    private void SaveHighScores(List<HighScoreEntry> highScores)
    {
        HighScoreList highScoresToSave = new HighScoreList { highScores = highScores };
        string highScoresJson = JsonUtility.ToJson(highScoresToSave);
        PlayerPrefs.SetString("highScores", highScoresJson);
        PlayerPrefs.Save();
    }

    private HighScoreList LoadHighScores()
    {
        string highScoresJson = PlayerPrefs.GetString("highScores");
        HighScoreList highScoresToLoad = JsonUtility.FromJson<HighScoreList>(highScoresJson);
        return highScoresToLoad;
    }

    public void ResetHighScores()
    {
        highScores = new List<HighScoreEntry>();
        highScoreTransforms = new List<Transform>();

        SaveHighScores(highScores);
    }

    public void PopulateHighScoresWithFakeData()
    {
        List<HighScoreEntry> tempHighScores = new List<HighScoreEntry>() {
            new HighScoreEntry { date = new DateTime(2023, 6, 22).ToString("M/d/yy"), score = 12345},
            new HighScoreEntry { date = new DateTime(2023, 5, 30).ToString("M/d/yy"), score = 4654},
            new HighScoreEntry { date = new DateTime(2023, 5, 2).ToString("M/d/yy"), score = 7656},
            new HighScoreEntry { date = new DateTime(2023, 4, 16).ToString("M/d/yy"), score = 334},
            new HighScoreEntry { date = new DateTime(2023, 3, 17).ToString("M/d/yy"), score = 980},
            new HighScoreEntry { date = new DateTime(2023, 3, 24).ToString("M/d/yy"), score = 8},
            new HighScoreEntry { date = new DateTime(2023, 3, 29).ToString("M/d/yy"), score = 11223},
            new HighScoreEntry { date = new DateTime(2023, 1, 8).ToString("M/d/yy"), score = 12124},
            new HighScoreEntry { date = new DateTime(2023, 1, 9).ToString("M/d/yy"), score = 12124},
            new HighScoreEntry { date = new DateTime(2023, 1, 8).ToString("M/d/yy"), score = 1745},
        };

        foreach (HighScoreEntry entry in tempHighScores)
        {
            InsertHighScoreIntoListSorted(entry, highScores);
        }

        SaveHighScores(highScores);
    }
}
