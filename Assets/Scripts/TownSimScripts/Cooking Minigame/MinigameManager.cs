using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager i;

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
    public List<RecipeData> allRecipes = new List<RecipeData>();
    public GameObject recipeGrid;
    public GameObject recipeButtonPrefab;
    public RecipeData selectedRecipe;

    [Header("Scoring")]
    public List<float> minigameScores = new List<float>();

    private bool isProblemRun;

    public void UpdateCurrencyDisplay(float finalScore) => StartCoroutine(EndscreenAnimations(finalScore));

    void Start()
    {
        i = this;
        gameManager = TownGameManager.i;
    }

    public void StartProblemMinigame(CompleteCharacterData problemCharacter)
    {
        isProblemRun = true;
        characterSelectionMenu.selectedCharacter = problemCharacter;
        NextMinigameScene();
    }

    public void NextMinigameScene()
    {
        gameScenes[currentScene].gameObject.SetActive(false);
        currentScene++;
        gameScenes[currentScene].gameObject.SetActive(true);
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
            else if (!confirmWindowVisible && characterSelectionMenu.selectedCharacter.Name != "")
            {
                windowText.text = "Start cooking with " + characterSelectionMenu.selectedCharacter.Name + "?";

                confirmWindow.SetActive(true);
                confirmWindowVisible = !confirmWindowVisible;
            }
        }
    }

    public void TotalScore(float newScore)
    {
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
        if (!isProblemRun)
        {
            ToggleConfirmWindow();
        }

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
        isProblemRun = false;
    }

    public void UpdateHappinessDisplay(float finalScore)
    {
        var happinessToAdd = maxHappiness * (finalScore / 100);
        CharacterManager.i.IncreaseHappiness(characterSelectionMenu.selectedCharacter.ID, happinessToAdd);
    }

    

    IEnumerator EndscreenAnimations(float finalScore)
    {
        yield return new WaitForSeconds(2);

        //gameManager._currency += maxCurrency * (finalScore / 100);

        yield return new WaitForSeconds(2);
    }
}
