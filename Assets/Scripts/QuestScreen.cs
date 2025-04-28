using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestScreen : MonoBehaviour
{
    public Quest associatedQuest;

    [Header("Screen Data")]

    public Button backButton;

    public Image questIcon;
    public TMP_Text recommendedLevelText;

    [Header("Character Select")]
    public GameObject characterSelectOne;
    public CharacterData selectedCharacterOne;
    public Image characterDisplayOne;

    public GameObject characterSelectTwo;
    public CharacterData selectedCharacterTwo;
    public Image characterDisplayTwo;

    void Start()
    {
        ToggleCharacterSelect(characterSelectOne, false);
        ToggleCharacterSelect(characterSelectTwo, false);

    }
    public void ToggleCharacterSelect(GameObject toggleWindow, bool isActive)
    {
        toggleWindow.SetActive(isActive);
    }

    


}
