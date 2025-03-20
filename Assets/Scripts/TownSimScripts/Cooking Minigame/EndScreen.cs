using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    public MinigameManager manager;
    public TMP_Text finalScore;

    float tallyScore;

    void OnEnable()
    {
        //calculates score

        foreach(float score in manager.minigameScores)
        {
            tallyScore += score;
        }

        tallyScore /= manager.minigameScores.Count;

        // updates currency and happiness displays instantly, add delay animation method later.
        DisplayScore(tallyScore);

        manager.UpdateCurrencyDisplay(tallyScore);
        manager.UpdateHappinessDisplay(tallyScore);
    }

       private void DisplayScore(float score)
    {
        score = Mathf.Round(score);
        finalScore.text = "Final Score: " + score.ToString();
    }
}
