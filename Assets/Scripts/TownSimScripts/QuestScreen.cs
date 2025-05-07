using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestScreen : MonoBehaviour
{
    //public BFQManager questManager;
    public Quest associatedQuest;

    [Header("Quest Data")]

    public Button backButton;

    public Image questIcon;
    public TMP_Text recommendedLevelText;
    public TMP_Text relationshipLevelText;

    public TMP_Text timerText;
    public TMP_Text percentText;

    public Button startButton;

    [Header("Character Select")]

    public CharacterSelectionMenu selectedCharacterOne;
    public Image characterDisplayOne;

    public CharacterSelectionMenu selectedCharacterTwo;
    public Image characterDisplayTwo;

    public Sprite defaultSprite;

    [Header("Character Select Buttons")]
    public Button toggleButtonOne;
    public Button toggleButtonTwo;

    public Button backButtonOne;
    public Button backButtonTwo;


    void Start()
    {
        //questManager = BFQManager.i;

        ToggleCharacterSelect(selectedCharacterOne.gameObject, false);
        ToggleCharacterSelect(selectedCharacterTwo.gameObject, false);

        toggleButtonOne.onClick.AddListener(() => ToggleCharacterSelect(selectedCharacterOne.gameObject, true));
        toggleButtonTwo.onClick.AddListener(() => ToggleCharacterSelect(selectedCharacterTwo.gameObject, true));

        backButtonOne.onClick.AddListener(() => ToggleCharacterSelect(selectedCharacterOne.gameObject, false));
        backButtonTwo.onClick.AddListener(() => ToggleCharacterSelect(selectedCharacterTwo.gameObject, false));


        recommendedLevelText.text = "Recommended Level: " + associatedQuest.relationshipRequirement;

        string[] tempArray = associatedQuest.completionTime.ToString("F2").Split(char.Parse("."));

        timerText.text = tempArray[0] + ":" + tempArray[1] + ":00";

    }

    private void OnDisable()
    {
        selectedCharacterOne.selectedCharacter = null; 
        selectedCharacterTwo.selectedCharacter = null;
        SetIcon(characterDisplayOne);
        SetIcon(characterDisplayTwo);
        relationshipLevelText.text = "Relationship Level: ???";
        percentText.text = "???";
    }

    private void Update()
    {
        if (selectedCharacterOne.selectedCharacter != null)
        {
            SetIcon(characterDisplayOne, selectedCharacterOne.selectedCharacter.characterIcon);
        }
        if (selectedCharacterTwo.selectedCharacter != null)
        {
            SetIcon(characterDisplayTwo, selectedCharacterTwo.selectedCharacter.characterIcon);
        }

        //update relationship text

        if(selectedCharacterOne.selectedCharacter != null && selectedCharacterTwo.selectedCharacter != null)
        {
            relationshipLevelText.text = "Relationship Level: " + Mathf.Floor(selectedCharacterOne.selectedCharacter.relationships[selectedCharacterTwo.selectedCharacter]).ToString();

            percentText.text = (Mathf.Floor(selectedCharacterOne.selectedCharacter.relationships[selectedCharacterTwo.selectedCharacter]) / associatedQuest.relationshipRequirement * 100).ToString("F0") + "%";
        }

    }
    public void ToggleCharacterSelect(GameObject toggleWindow, bool isActive)
    {
        toggleWindow.SetActive(isActive);
    }

    void SetIcon(Image newImage, Sprite newSprite =null)
    {
        if(newSprite == null)
        {
            newImage.sprite = defaultSprite;
        }
        else
        {
            newImage.sprite = newSprite;
        }
    }
    


}
