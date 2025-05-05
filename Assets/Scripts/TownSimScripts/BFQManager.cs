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
    public CharacterData charOne;
    public CharacterData charTwo;

    [Header("Results Screen")]

    public BFQResultsScreen resultsScreen;
    public bool questSuccess;

    void Awake()
    {
        i= this;

    }

    private void OnEnable()
    {
        if( questState == State.Selection)
        {
            selectionScreen.SetActive(true);
            inProgressScreen.gameObject.SetActive(false);
            resultsScreen.gameObject.SetActive(false);
        }
        else if( questState == State.InProgress)
        {
            selectionScreen.SetActive(false);
            inProgressScreen.gameObject.SetActive(true);
            resultsScreen.gameObject.SetActive(false);
        }
    }
    public void SelectQuest(Quest newQuest)
    {
        selectedQuest = newQuest;
    }

    public void StartQuest(QuestScreen questScreen)
    {
        //if there are two characters selected, transition to quest
        //add a confirm window first if time and put the check there instead

        if (questScreen.selectedCharacterOne.selectedCharacter != null && questScreen.selectedCharacterTwo.selectedCharacter != null)
        {
            questState = State.InProgress;

            charOne = questScreen.selectedCharacterOne.selectedCharacter;
            charTwo = questScreen.selectedCharacterTwo.selectedCharacter;

            selectionScreen.SetActive(false);

            inProgressScreen.SetTime(selectedQuest);
            inProgressScreen.gameObject.SetActive(true);

            inProgressScreen.iconOne.sprite= charOne.characterIcon;
            inProgressScreen.iconTwo.sprite = charTwo.characterIcon;

        }
    }

    public void StartResults()
    {
        questState = State.Results;

        //activate screen and set icons
        inProgressScreen.gameObject.SetActive(false);
        resultsScreen.gameObject.SetActive(true);

        resultsScreen.treasureChest.sprite = resultsScreen.chestClosed;

        float successChance = (charOne.relationships[charTwo] / selectedQuest.relationshipRequirement ) * 100;
        
        bool succeeded = (Random.Range(0f, 100f) <= successChance);

        if (succeeded) {
            selectedQuest.completed = true;
        }

        StartCoroutine(resultsScreen.ResultsAnimation(succeeded, selectedQuest, charOne, charTwo));
    }

    public void ResetBFQ()
    {
        questState = State.Selection;
    }
}
