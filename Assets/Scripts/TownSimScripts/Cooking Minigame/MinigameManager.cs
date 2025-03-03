using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinigameManager : MonoBehaviour
{
    public TownGameManager gameManager;

    [Header("Character Select")]
    public CharacterData selectedCharacter;
    public GameObject characterButtonPrefab;
    public GameObject characterSelectionGrid;



    // Start is called before the first frame update
    void Start()
    {
        GenerateCharacterSelect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateCharacterSelect()
    {
        foreach (CharacterData character in gameManager.characterManager.allCharacters)
        {
            //make their icons dawg
            GameObject newIcon = Instantiate(characterButtonPrefab, characterSelectionGrid.transform);
            newIcon.GetComponent<Button>().onClick.AddListener(() => SelectCharacter(character));
            newIcon.GetComponentInChildren<Image>().sprite = character.characterIcon;

        }


    }

    private void SelectCharacter(CharacterData character) { 
    
        selectedCharacter = character;

    }
}
