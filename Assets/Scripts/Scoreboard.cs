using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    private Transform scoreContainer;
    private Transform entryTemplate;

    public float templateHeight = 20f;

    private void Awake()
    {
        scoreContainer = transform.Find("ScoreContainer");
        entryTemplate = transform.Find("ScoreContainer/EntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        for (int i = 0; i < 10; i++)
        {
            Transform entryTransform = Instantiate(entryTemplate, scoreContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
            entryTransform.gameObject.SetActive(true);

            int rank = i + 1;

            entryTransform.Find("Position").GetComponent<TextMeshProUGUI>().text = rank + ".";
            entryTransform.Find("Date").GetComponent<TextMeshProUGUI>().text = Random.Range(1, 13) + "/" + Random.Range(1, 32) + "/23";
            entryTransform.Find("Score").GetComponent<TextMeshProUGUI>().text = Random.Range(10000, 200000).ToString("#,#");
        }
    }
}
