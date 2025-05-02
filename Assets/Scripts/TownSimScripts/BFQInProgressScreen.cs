using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class BFQInProgressScreen : MonoBehaviour
{
    public BFQManager questManager;

    [Header("Timer")]
    public DateTime startTime;
    public DateTime currentTime;
    public DateTime endTime;

    public TMP_Text timerText;
    public GameObject completeButton;

    public Image iconOne;
    public Image iconTwo;


    void OnEnable()
    {
        timerText.gameObject.SetActive(true);
        completeButton.SetActive(false);
    }
    void Start()
    {
        questManager = BFQManager.i;
    }

    void Update()
    {
        currentTime = DateTime.Now;

        if (endTime != null && DateTime.Compare(currentTime, endTime) < 0)
        {
            timerText.text = (endTime - currentTime).ToString(@"hh\:mm\:ss");
        }
        else if (endTime != null && DateTime.Compare(currentTime, endTime) > 0) { 
            timerText.gameObject.SetActive(false);
            completeButton.SetActive(true);
        }
    }
    public void SetTime(Quest associatedQuest)
    {
        startTime = DateTime.Now;
        endTime = DateTime.Now.AddHours(associatedQuest.completionTime);

        Debug.Log(startTime + " and end at " + endTime);

    }


}
