using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinigameTimer : MonoBehaviour
{
    public MinigameManager minigameManager;

    [Header("Timer")]

    public float totalTime;
    public bool timerActive = false;

    public float endTime;
    public float startTime;

    public TMP_Text timerText;

    private float tempTime;

    [Header("Progress Bar")]
    public GameObject progressBar;
    public float progressScore;

    [Header("Audio")]
    [SerializeField] private Sound tickSFX;



    // Start is called before the first frame update
    void OnEnable()
    {
        tickSFX = Instantiate(tickSFX);

        timerText = GetComponentInChildren<TMP_Text>();

        startTime = Time.time;
        endTime = Time.time + totalTime;

        timerActive = true;

        progressBar.GetComponent<RectTransform>().sizeDelta = new Vector2(progressBar.GetComponent<RectTransform>().sizeDelta.x, 0);


        //BAD BAD BAD KILL KILL KILL
        minigameManager = FindFirstObjectByType<MinigameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive && Time.time > endTime)
        {
            timerText.text = "0";

            EndMinigame();
        }

        if (timerActive && Time.time <= endTime + .5)
        {
            //play tick when number changes
            if (tempTime != 0 && tempTime != Mathf.Ceil(endTime - Time.time))
            {
                tickSFX.Play();
            }

            timerText.text = Mathf.Ceil(endTime - Time.time).ToString();

            tempTime = Mathf.Ceil(endTime - Time.time);
            
        }

    }

    public void AddProgress(float score)
    {
        if (timerActive)
        {
             if( progressScore + score < 100)
            {
                progressScore += score;
            }
            else
            {
                progressScore = 100;

                EndMinigame();
            }
            UpdateProgress();
        }
       
    }

    public void RemoveProgress(float score) 
    {
        if (timerActive)
        {
            if (progressScore - score >= 0) 
            {
                progressScore -= score;
            }
            else
            {
                progressScore = 0;
            }
            UpdateProgress();
        }
        
    }

    public void UpdateProgress()
    {
        float newHeight = progressBar.transform.parent.GetComponent<RectTransform>().sizeDelta.y * (progressScore / 100);

        progressBar.GetComponent<RectTransform>().sizeDelta = new Vector2(progressBar.GetComponent<RectTransform>().sizeDelta.x, newHeight);
    }


    public void EndMinigame()
    {
        minigameManager.TotalScore(progressScore);

        timerActive = false;
    }
}
