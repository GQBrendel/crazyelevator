using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameJoltAPI : MonoBehaviour
{
    public void AddScore(int scoreValue)
    {
        if(scoreValue == 0)
        {
            return;
        }
        string scoreText = scoreValue.ToString(); // A string representing the score to be shown on the website.
        int tableID = 0; // Set it to 0 for main highscore table.
        string extraData = ""; // This will not be shown on the website. You can store any information.
        GameJolt.API.Scores.Add(scoreValue, scoreText, tableID, extraData, (bool success) => {
            Debug.Log(string.Format("Score Add {0}.", success ? "Successful" : "Failed"));
        });
    }
}
