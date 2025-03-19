using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterHouse : MonoBehaviour
{

    public CharacterData associatedCharacter;

    [Header("Status Menu")]

    public GameObject houseStatusMenu;

    private bool statusEnabled = false;
    public TMP_Text displayName;
    public TMP_Text statusButtonText;

    private void Start()
    {

    }
    private void OnEnable()
    {
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

    }
    public void UpdateRelationships()
    {

    }
}
