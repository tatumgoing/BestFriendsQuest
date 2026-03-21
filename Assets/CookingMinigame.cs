using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingMinigame : MinigameController
{
    [SerializeField] private GameObject _characterSelectScreen;
    [SerializeField] private RecipeSelector _recipeSelector;
    [SerializeField] private RestrauntController _areaController;

    [Header("Minigames")]
    [SerializeField] private StirMinigame _stirMinigame;

    //TESTING
    [Header("TESTING")]
    [SerializeField] private List<RecipeData> _recipes;
    [SerializeField] private ID _testID = new ID(8126);
    [SerializeField] private RecipeData _testRecipe;

    private ID _selectedCharacter = new ID(0);

    private void OnEnable()
    {
        _recipeSelector.gameObject.SetActive(false);

        if (CharacterManager.i) OpenCharacterSelect();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
            _selectedCharacter = _testID;
            StartCooking(_testRecipe);
        }
    }

    private void OpenCharacterSelect()
    {
        _characterSelectScreen.SetActive(true);
    }

    /// <summary>
    /// Called from the confirm window of the character selection menu
    /// </summary>
    public override void SelectCharacter(ID id)
    {
        base.SelectCharacter(id);

        _selectedCharacter = id;
        _characterSelectScreen.SetActive(false);
        ShowRecipeOptions();
    }

    public void ShowRecipeOptions()
    {
        _recipeSelector.ShowRecipes(_recipes);
    }

    public void StartCooking(RecipeData recipe)
    {
        _recipeSelector.gameObject.SetActive(false);
        _areaController.SpawnCharacter(_selectedCharacter);

        _stirMinigame.StartStirring();
        //MinigameManager.i.NextMinigameScene();
    }
}
