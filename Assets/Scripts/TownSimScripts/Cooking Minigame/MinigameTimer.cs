using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinigameTimer : MonoBehaviour
{
    public MinigameManager minigameManager;

    [Header("Timer")]

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

        progressBar.GetComponent<RectTransform>().sizeDelta = new Vector2(progressBar.GetComponent<RectTransform>().sizeDelta.x, 0);


        //BAD BAD BAD KILL KILL KILL
        minigameManager = FindFirstObjectByType<MinigameManager>();


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
        UpdateProgress();
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
        UpdateProgress();
    }

    public void UpdateProgress()
    {
        float newHeight = progressBar.transform.parent.GetComponent<RectTransform>().sizeDelta.y * (progressScore / 100);

        progressBar.GetComponent<RectTransform>().sizeDelta = new Vector2(progressBar.GetComponent<RectTransform>().sizeDelta.x, newHeight);
    }

}
