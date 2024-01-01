using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    //Private variable that holds the score
    private int score = 0;
    //Private reference to the score text component
    [SerializeField] private TextMeshProUGUI scoreText;
    
    //Adds 1 to the score counter
    public void AddScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    //Gets the score and returns this in integer format
    //public int GetScore() => score;

    //Testing of Score Counter - Prints the score to the console
    public void GetScore()
    {
        Debug.Log(score);
    }

}
