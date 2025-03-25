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

    [Header("Happiness and Money Toggles")]

    public float maxHappiness;
    public float maxCurrency;

    [Header("Character Select")]
    public CharacterSelectionMenu characterSelectionMenu;

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
    public CompletionText completionText;

    [Header("Scoring")]
    public List<float> minigameScores = new List<float>();

    [Header("End Screen")]
    public GameObject endScreen;

    public GameObject happinessMeter;
    public Image endScreenIcon;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = TownGameManager.i;

        GenerateRecipeSelect();
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

        if (gameScenes[currentScene].GetComponentInChildren<CompletionText>() != null) {
            
            completionText = gameScenes[currentScene].GetComponentInChildren<CompletionText>();
            completionText.gameObject.SetActive(false);
        }
        
    }

    public void ToggleConfirmWindow()
    {
        if (characterSelectionMenu.selectedCharacter != null)
        {
            if (confirmWindowVisible)
            {
                confirmWindow.SetActive(false);
                confirmWindowVisible = !confirmWindowVisible;
            }
            else if (!confirmWindowVisible && characterSelectionMenu.selectedCharacter.characterName != "")
            {
                windowText.text = "Start cooking with " + characterSelectionMenu.selectedCharacter.characterName + "?";

                confirmWindow.SetActive(true);
                confirmWindowVisible = !confirmWindowVisible;
            }
        }
    }

    public void TotalScore(float newScore)
    {
        //Debug.Log("Totalling Score");

        completionText.gameObject.SetActive(true);

        minigameScores.Add(newScore);
        StartCoroutine(StartNextMinigameDelay());
    }

    IEnumerator StartNextMinigameDelay()
    {
        completionText.PlayCompletionSFX();

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

        characterSelectionMenu.selectedCharacter = null;
        selectedRecipe = null;

        minigameScores.Clear();

    }

    public void UpdateHappinessDisplay(float finalScore)
    {
        endScreenIcon.sprite= characterSelectionMenu.selectedCharacter.characterIcon;

        float newWidth = happinessMeter.transform.parent.GetComponent<RectTransform>().sizeDelta.x * (characterSelectionMenu.selectedCharacter.happiness / 100);
        happinessMeter.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, happinessMeter.GetComponent<RectTransform>().sizeDelta.y);

        characterSelectionMenu.selectedCharacter.happiness += (maxHappiness * (finalScore / 100));
        characterSelectionMenu.selectedCharacter.happiness= Mathf.Clamp(characterSelectionMenu.selectedCharacter.happiness, 0, 100);

    }

    public void UpdateCurrencyDisplay(float finalScore)
    {

        //gameManager.currency += maxCurrency * (finalScore / 100);

        StartCoroutine(EndscreenAnimations(finalScore));
    }

    IEnumerator EndscreenAnimations(float finalScore)
    {

        yield return new WaitForSeconds(2);

        gameManager.currency += maxCurrency * (finalScore / 100);

        yield return new WaitForSeconds(2);

        float newWidth = happinessMeter.transform.parent.GetComponent<RectTransform>().sizeDelta.x * (characterSelectionMenu.selectedCharacter.happiness / 100);
        happinessMeter.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, happinessMeter.GetComponent<RectTransform>().sizeDelta.y);

    }

}
