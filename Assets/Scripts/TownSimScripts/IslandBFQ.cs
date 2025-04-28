using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandBFQ : MonoBehaviour
{
    public Quest associatedQuest;

    public GameObject screenPrefab;

    QuestScreen associatedScreen;

    void Start()
    {

        //create quest Screen

        associatedScreen = Instantiate(screenPrefab, this.transform.parent).GetComponent<QuestScreen>();
        associatedScreen.questIcon.sprite = associatedQuest.unlockedItem.sprite;

        associatedScreen.associatedQuest = associatedQuest;
        associatedScreen.backButton.onClick.AddListener(() => ToggleScreen(false));

        ToggleScreen(false);

        Button tempButton = GetComponentInChildren<Button>();

        tempButton.onClick.AddListener(() => ToggleScreen(true));
    }

    void ToggleScreen(bool isActive)
    {
        if (associatedScreen != null) { 
            associatedScreen.gameObject.SetActive(isActive);
        }
    }

   
}
