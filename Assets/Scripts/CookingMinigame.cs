using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingMinigame : MinigameController
{
    [SerializeField] private GameObject _characterSelectScreen;
    [SerializeField] private GameObject _recipientSelectScreen;
    [SerializeField] private RecipeSelector _recipeSelector;
    [SerializeField] private RestrauntController _areaController;
    [SerializeField] private SubgameController _subgameController;
    [SerializeField] private GameObject _startButton;
    [SerializeField] private GameObject _backButton;

    [Header("Minigames")]
    [SerializeField] private StirMinigame _stirMinigame;

    //TESTING
    [Header("TESTING")]
    [SerializeField] private List<RecipeData> _recipes;
    [SerializeField] private ID _testID = new ID(8126);
    [SerializeField, DisplayInspector] private RecipeData _testRecipe;

    private ID _selectedCharacter = new ID(0);
    private ID _selectedRecipient = new ID(0);
    private GameObject _spawnedCharacter;

    private void OnEnable()
    {
        if (_spawnedCharacter != null) Destroy(_spawnedCharacter.gameObject);

        _subgameController.gameObject.SetActive(false);
        _recipeSelector.gameObject.SetActive(false);
        _recipientSelectScreen.SetActive(false);
        _characterSelectScreen.SetActive(false);

        _backButton.SetActive(true);
        _startButton.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
            _startButton.SetActive(false);
            _backButton.SetActive(false);
            SelectPrimaryCharacter(_testID);
            SelectRecipient(new ID(2216));
            StartCooking(_testRecipe);
        }
    }

    public void ResetKitchen()
    {
        _subgameController.gameObject.SetActive(false);
        Destroy(_spawnedCharacter.gameObject);
        _startButton.SetActive(true);
        _backButton.SetActive(true);
    }

    public void OpenCharacterSelect()
    {
        _backButton.SetActive(false);
        _startButton.SetActive(false);
        _characterSelectScreen.SetActive(true);
    }

    /// <summary>
    /// Called from the confirm window of the character selection menu
    /// </summary>
    public override void SelectPrimaryCharacter(ID id)
    {
        base.SelectPrimaryCharacter(id);

        _selectedCharacter = id;
        _characterSelectScreen.SetActive(false);
        _recipientSelectScreen.SetActive(true);

        _spawnedCharacter = _areaController.SpawnCharacter(_selectedCharacter);
    }

    public override void SelectRecipient(ID id)
    {
        base.SelectRecipient(id);

        _selectedRecipient = id;
        _recipientSelectScreen.SetActive(false);
        ShowRecipeOptions();
    }

    public void ShowRecipeOptions()
    {
        _recipeSelector.ShowRecipes(_recipes);
    }

    public void StartCooking(RecipeData recipe)
    {
        _recipeSelector.gameObject.SetActive(false);


        _subgameController.StartMinigame(recipe, _selectedCharacter, _selectedRecipient);

        //_stirMinigame.StartStirring();
        //MinigameManager.i.NextMinigameScene();
    }
}
