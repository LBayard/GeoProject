using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayScore : MonoBehaviour
{
    private ScoreKeeper scoreKeeper;
    // Start is called before the first frame update
    void Start()
    {
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        string displayTime = string.Format("{0:00}:{1:00}", scoreKeeper.GetMinutes(), scoreKeeper.GetSeconds());
        GetComponent<TextMeshProUGUI>().text = $"Your time is: {displayTime}";
    }
}
