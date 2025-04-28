using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectionMenu : MonoBehaviour
{
    public bool isQuest= false;

    [Header("Managers")] 
    
    public TownGameManager gameManager;

    [Header("Selected Character")]

    public CharacterData selectedCharacter;

    [Header("Layout")]
    public GameObject characterButtonPrefab;
    public GameObject characterSelectionGrid;

    public List<GameObject> characterSelectionButtons = new List<GameObject>();


    // Start is called before the first frame update

    void Start()
    {
        gameManager = TownGameManager.i;

        GenerateCharacterSelect();

    }
    void OnEnable()
    {
        
    }

    public void GenerateCharacterSelect()
    {
        foreach (Transform child in characterSelectionGrid.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (CharacterData character in gameManager.characterManager.allCharacters)
        {
            if (!isQuest)
            {
                MakeIcon(character);
            }
            else if(character.happiness>= 100)
            {
                MakeIcon(character);
            }
            else
            {
                MakeUnselectableIcon(character);
            }
        }

    }

    public void MakeIcon(CharacterData character)
    {
        GameObject newIcon = Instantiate(characterButtonPrefab, characterSelectionGrid.transform);

        newIcon.GetComponent<Button>().onClick.AddListener(() => SelectCharacter(character));

        newIcon.GetComponent<Button>().onClick.AddListener(() => SelectButton(newIcon));

        newIcon.GetComponent<Image>().sprite = character.characterIcon;

        characterSelectionButtons.Add(newIcon);
    }

    public void MakeUnselectableIcon(CharacterData character)
    {
        GameObject newIcon = Instantiate(characterButtonPrefab, characterSelectionGrid.transform);

        newIcon.GetComponent<Button>().interactable= false;

        newIcon.GetComponent<Image>().sprite = character.characterIcon;

        characterSelectionButtons.Add(newIcon);
    }

    private void SelectCharacter(CharacterData character)
    {

        selectedCharacter = character;

    }

    public void SelectButton(GameObject button)
    {
        //Debug.Log(button);
        foreach (GameObject resetButton in characterSelectionButtons)
        {
            if(resetButton != button)
            {
                 resetButton.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            }
        }

        button.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1);
    }
}
