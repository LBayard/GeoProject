using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    private int currentScore = 0;

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
    public int GetScore()
    {
        return currentScore;
    }

    public void AddToScore(int points)
    {
        currentScore += points;
        Mathf.Clamp(points, 0, int.MaxValue);
        Debug.Log($"Score: {GetScore()}");
    }

    public void ResetScore()
    {
        currentScore = 0;
    }
}
