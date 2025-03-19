using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class CharacterHouse : MonoBehaviour
{

    public CharacterData associatedCharacter;

    [Header("Status Menu")]

    public GameObject houseStatusMenu;

    private bool statusEnabled = false;
    public TMP_Text displayName;
    public TMP_Text statusButtonText;

    public GameObject houseProgressBar;

    public GameObject relationshipPrefab;
    public GameObject relationshipContainer;



    private void Start()
    {

    }
    private void OnEnable()
    {
        statusEnabled = false;
        houseStatusMenu.SetActive(false);

        UpdateHappiness();
        UpdateRelationships();

        displayName.text = associatedCharacter.characterName;
    }

    public void SetHouseCharacter(CharacterData character)
    {
        associatedCharacter = character;
    }

    public void ToggleStatusWindow()
    {
        if (statusEnabled)
        {
            houseStatusMenu.SetActive(false);
            statusEnabled = false;
            statusButtonText.text = "Status";
        }
        else
        {
            houseStatusMenu.SetActive(true); 
            statusEnabled = true;
            statusButtonText.text = "X";
        }
    }

    
    public void UpdateHappiness()
    {
        float newWidth = houseProgressBar.transform.parent.GetComponent<RectTransform>().sizeDelta.x * (associatedCharacter.happiness / 100);
        houseProgressBar.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, houseProgressBar.GetComponent<RectTransform>().sizeDelta.y);

    }
    public void UpdateRelationships()
    {
        foreach (CharacterData reloCharacter in associatedCharacter.relationships.Keys)
        {
            RelationshipBanner newBanner = Instantiate(relationshipPrefab, relationshipContainer.transform).GetComponent<RelationshipBanner>();
            newBanner.icon.sprite = reloCharacter.characterIcon;
            newBanner.nameRelo.text = reloCharacter.characterName;
            newBanner.level.text = associatedCharacter.relationships[reloCharacter].ToString();
            newBanner.status.text = "Testing";
        } 
    }
}
