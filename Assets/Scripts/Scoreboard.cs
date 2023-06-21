using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    void OnGUI()
    {
        GUI.Label(new Rect(50, 50, 100, 20), "Score: 10");
    }
}
