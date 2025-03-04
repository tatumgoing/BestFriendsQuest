using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinigameTimer : MonoBehaviour
{
    public MinigameManager minigameManager;

    public float totalTime;
    private bool startTimer = false;

    public float endTime;
    public float startTime;

    public TMP_Text timerText;

    [Header("Progress Bar")]
    public GameObject progressBar;
    public float progressScore;


    // Start is called before the first frame update
    void OnEnable()
    {
        timerText = GetComponentInChildren<TMP_Text>();

        startTime = Time.time;
        endTime = Time.time + totalTime;

        startTimer = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer && Time.time > endTime)
        {
            minigameManager.TotalScore(progressScore);

            startTimer = false;
        }

        if (Time.time <= endTime + .5)
        {
            timerText.text = Mathf.Ceil(endTime - Time.time).ToString();

        }

    }

    public void AddProgress(float score)
    {
        if( progressScore + score <= 100)
        {
            progressScore += score;
        }
        else
        {
            progressScore = 100;
        }
        Debug.Log(progressScore);
    }

    public void RemoveProgress(float score) 
    {
        if (progressScore - score >= 0) {
            progressScore -= score;
        }
        else
        {
            progressScore = 0;
        }
        Debug.Log(progressScore);
    }

}
