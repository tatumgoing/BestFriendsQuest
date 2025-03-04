using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinigameManager : MonoBehaviour
{
    public TownGameManager gameManager;

    public List<GameObject> gameScenes = new List<GameObject>();
    private int currentScene;

    [Header("Character Select")]
    public CharacterData selectedCharacter;
    public GameObject characterButtonPrefab;
    public GameObject characterSelectionGrid;

    public GameObject confirmWindow;
    public TMP_Text windowText;
    private bool confirmWindowVisible = false;

    [Header("Cooking Minigame")]
    public GameObject tempIcon;


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

    public void NextMinigameScene()
    {
        gameScenes[currentScene].gameObject.SetActive(false);

        currentScene++;

        gameScenes[currentScene].gameObject.SetActive(true);

        tempIcon.GetComponent<Image>().sprite= selectedCharacter.characterIcon;
    }

    public void ToggleConfirmWindow()
    {            
        windowText.text = "Start cooking with " + selectedCharacter.characterName + "?";

        if (confirmWindowVisible)
        {
            confirmWindow.SetActive(false);
            confirmWindowVisible= !confirmWindowVisible;
        }
        else if (!confirmWindowVisible)
        {
            confirmWindow.SetActive(true);
            confirmWindowVisible = !confirmWindowVisible;
        }
    }
}
