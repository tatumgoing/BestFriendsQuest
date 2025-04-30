using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandBFQ : MonoBehaviour
{
    public BFQManager questManager;


    public Quest associatedQuest;

    public GameObject screenPrefab;

    QuestScreen associatedScreen;

    void Start()
    {
        questManager = BFQManager.i;

        //create quest Screen

        associatedScreen = Instantiate(screenPrefab, this.transform.parent).GetComponent<QuestScreen>();
        associatedScreen.questIcon.sprite = associatedQuest.unlockedItem.sprite;

        associatedScreen.associatedQuest = associatedQuest;
        associatedScreen.backButton.onClick.AddListener(() => ToggleScreen(false));

        ToggleScreen(false);

        Button tempButton = GetComponentInChildren<Button>();

        tempButton.onClick.AddListener(() => ToggleScreen(true));
        tempButton.onClick.AddListener(() => questManager.SelectQuest(associatedQuest));

    }

    void ToggleScreen(bool isActive)
    {
        if (associatedScreen != null) { 
            associatedScreen.gameObject.SetActive(isActive);
        }
    }

   
}
