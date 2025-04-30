using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { Selection, InProgress, Results }

public class BFQManager : MonoBehaviour
{
    public static BFQManager i;

    public State questState;

    [Header("Selection Screen")]

    public GameObject selectionScreen;
    public Quest selectedQuest;

    [Header("In Progress Screen")]

    public BFQInProgressScreen inProgressScreen;

    [Header("Results Screen")]

    public GameObject resultsScreen;

    void Start()
    {
        i= this;

    }
    public void SelectQuest(Quest newQuest)
    {
        selectedQuest = newQuest;
    }

    public void StartQuest()
    {
        questState = State.InProgress;
        selectionScreen.SetActive(false);

        inProgressScreen.SetTime(selectedQuest);
        inProgressScreen.gameObject.SetActive(true);
    }
}
