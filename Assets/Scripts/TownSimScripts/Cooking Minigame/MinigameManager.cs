using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinigameManager : MonoBehaviour
{
    public TownGameManager gameManager;
    public GameObject minigameUIContainer;

    public List<GameObject> gameScenes = new List<GameObject>();
    private int currentScene;

    [Header("Character Select")]
    public CharacterData selectedCharacter;
    public GameObject characterButtonPrefab;
    public GameObject characterSelectionGrid;

    public GameObject confirmWindow;
    public TMP_Text windowText;
    private bool confirmWindowVisible = false;

    [Header("Recipes")]
    public List<Recipe> allRecipes = new List<Recipe>();
    public GameObject recipeGrid;
    public GameObject recipeButtonPrefab;
    public Recipe selectedRecipe;

    [Header("Cooking Minigame")]
    public GameObject tempIcon;
    public MinigameTimer currentTimer;

    [Header("Scoring")]
    public List<float> minigameScores = new List<float>();

    [Header("End Screen")]
    public GameObject endScreen;

    public GameObject happinessMeter;
    public Image endScreenIcon;


    // Start is called before the first frame update
    void Start()
    {
        GenerateCharacterSelect();
        GenerateRecipeSelect();
    }

    // Update is called once per frame
    void Update()
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
            //make their icons dawg
            GameObject newIcon = Instantiate(characterButtonPrefab, characterSelectionGrid.transform);
            newIcon.GetComponent<Button>().onClick.AddListener(() => SelectCharacter(character));
            newIcon.GetComponentInChildren<Image>().sprite = character.characterIcon;
        }


    }

    public void GenerateRecipeSelect()
    {
        foreach (Transform child in recipeGrid.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Recipe recipe in allRecipes)
        {
            //make their icons dawg
            GameObject newIcon = Instantiate(recipeButtonPrefab, recipeGrid.transform);
            newIcon.GetComponent<Button>().onClick.AddListener(() => SelectRecipe(recipe));
            newIcon.GetComponentInChildren<Image>().sprite = recipe.Icon;
            newIcon.GetComponentInChildren<TMP_Text>().text = recipe.Name;
        }


    }

    private void SelectCharacter(CharacterData character) { 
    
        selectedCharacter = character;

    }

    private void SelectRecipe(Recipe recipe)
    {
        selectedRecipe = recipe;

        foreach (GameObject minigame in recipe.Minigames) {

            var newMinigame = Instantiate(minigame, minigameUIContainer.transform);
            newMinigame.SetActive(false);
            gameScenes.Add(newMinigame);

        }

        gameScenes.Add(endScreen);

        NextMinigameScene();
    }

    public void NextMinigameScene()
    {
        gameScenes[currentScene].gameObject.SetActive(false);

        currentScene++;

        gameScenes[currentScene].gameObject.SetActive(true);

        //assign new variables, could be unique method
        if (gameScenes[currentScene].GetComponentInChildren<MinigameTimer>() != null) {

            currentTimer = gameScenes[currentScene].GetComponentInChildren<MinigameTimer>();

        }

        if (gameScenes[currentScene].GetComponentInChildren<ChopMinigame>() != null)
        {
            gameScenes[currentScene].GetComponentInChildren<ChopMinigame>().tempIcon.GetComponent<Image>().sprite = selectedCharacter.characterIcon;
            //ew
        }
    }

    public void ToggleConfirmWindow()
    {
        if (selectedCharacter != null)
        {
            if (confirmWindowVisible)
            {
                confirmWindow.SetActive(false);
                confirmWindowVisible = !confirmWindowVisible;
            }
            else if (!confirmWindowVisible && selectedCharacter.characterName != "")
            {
                windowText.text = "Start cooking with " + selectedCharacter.characterName + "?";

                confirmWindow.SetActive(true);
                confirmWindowVisible = !confirmWindowVisible;
            }
        }
    }

    public void TotalScore(float newScore)
    {
        Debug.Log("Totalling Score");
        minigameScores.Add(newScore);
        StartCoroutine(StartNextMinigameDelay());
    }

    IEnumerator StartNextMinigameDelay()
    {
        yield return new WaitForSeconds(3);

        NextMinigameScene();
    }

    public void RestartGame()
    {
        ToggleConfirmWindow();

        gameScenes[currentScene].gameObject.SetActive(false);

        List<GameObject> gameScenesTemp = new List<GameObject>();

        gameScenesTemp.Add(gameScenes[0]);
        gameScenesTemp.Add(gameScenes[1]);

        gameScenes = gameScenesTemp;

        gameScenes[0].gameObject.SetActive(true);

        currentScene = 0;

        selectedCharacter = null;
        selectedRecipe = null;

        minigameScores.Clear();

    }

    public void UpdateHappinessDisplay()
    {
        endScreenIcon.sprite= selectedCharacter.characterIcon;

        float newWidth = happinessMeter.transform.parent.GetComponent<RectTransform>().sizeDelta.x * (selectedCharacter.happiness / 100);
        happinessMeter.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, happinessMeter.GetComponent<RectTransform>().sizeDelta.y);

    }

    public void UpdateCurrencyDisplay()
    {
        //new currency object and prefab to proceed
    }

}
