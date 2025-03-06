using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    public MinigameManager manager;
    public TMP_Text finalScore;

    float tallyScore;
    // Start is called before the first frame update
    void OnEnable()
    {
        
        foreach(float score in manager.minigameScores)
        {
            tallyScore += score;
        }

        tallyScore /= manager.minigameScores.Count;

        DisplayScore(tallyScore);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DisplayScore(float score)
    {
        score = Mathf.Round(score);
        finalScore.text = "Final Score: " + score.ToString();
    }
}
