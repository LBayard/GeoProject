using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    private float timerValue;

    static ScoreKeeper instance;

    private void Awake()
    {
        ManageSingleton();
    }

    void ManageSingleton()
    {
        if (instance != null)
        {
            //Very small chance that other objects grab and use gameobject,
            //so set active to false before destroying the game object
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = FindObjectOfType<ScoreKeeper>();
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddTime(float time)
    {
        timerValue += time;
    }

    public int GetMinutes()
    {
        return Mathf.FloorToInt(timerValue / 60);   
    }

    public int GetSeconds()
    {
        return Mathf.FloorToInt(timerValue % 60);
    }

    public void ResetScore()
    {
        timerValue = 0;
    }
}
