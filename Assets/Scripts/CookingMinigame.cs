using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingMinigame : MinigameController
{
    [SerializeField] private GameObject _characterSelectScreen;
    [SerializeField] private CharacterSelectionMenu _recipientSelectScreen;
    [SerializeField] private RecipeSelector _recipeSelector;
    [SerializeField] private RestrauntController _areaController;
    [SerializeField] private SubgameController _subgameController;
    [SerializeField] private GameObject _backButton;

    [Header("Subgames")]
    [SerializeField] private StirMinigame _stirMinigame;

    //===TESTING===
    [Header("TESTING")]
    [SerializeField] private ItemData _chefHat;
    [SerializeField] private List<RecipeData> _recipes;
    [SerializeField] private ID _testChefID = new ID(8126);
    [SerializeField] private ID _testRecipientID = new ID(8126);
    [SerializeField, DisplayInspector] private RecipeData _testRecipe;

    private ID _selectedCharacter = new ID(0);
    private ID _selectedRecipient = new ID(0);
    private bool _solvingProblem;
    private SpawnedCharacter _spawnedCharacter;

    // I, too, am testing
    private ID _displayedSelectedCharacter = new ID(0); //tracks character on the main menu

    public RestrauntController AreaController => _areaController;
    public override MinigameType GetMinigameType() => MinigameType.COOKING;
    public override Transform GetCamera() => _areaController.Camera.transform;

    private void OnEnable()
    {
        //set camera to the main camera 
        _areaController.ResetCamera();

        if (_spawnedCharacter != null) Destroy(_spawnedCharacter.gameObject);

        _subgameController.gameObject.SetActive(false);
        _recipeSelector.gameObject.SetActive(false);
        _recipientSelectScreen.gameObject.SetActive(false);
        _characterSelectScreen.SetActive(false);
        _solvingProblem = false;

        _backButton.SetActive(true);
        OpenCharacterSelect();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) QuickTest();

        //spawn characters as they are selected
        if (_characterSelectScreen.GetComponent<CharacterSelectionMenu>()._selectedCharacter != _displayedSelectedCharacter)
        {
            _areaController.SpawnCharacterSelect(_characterSelectScreen.GetComponent<CharacterSelectionMenu>()._selectedCharacter);
            _displayedSelectedCharacter = _characterSelectScreen.GetComponent<CharacterSelectionMenu>()._selectedCharacter;
        }
    }

    [ButtonMethod]
    public void QuickTest()
    {
        _backButton.SetActive(false);
        SelectPrimaryCharacter(_testChefID);
        SelectRecipient(_testRecipientID);
        StartCooking(_testRecipe);
    }

    public override void StartProblemMinigame(ID character)
    {
        gameObject.SetActive(true);
        _backButton.SetActive(false);

        _solvingProblem = true;

        SelectPrimaryCharacter(character);
    }

    /// <summary>
    /// Called from minigameResultsScreen after hitting 'continue' after solving a problem that requires playing a minigame
    /// sends the player back to the room of the character for some post-problem dialogue and rewards
    /// </summary>
    override public void CompleteProblem()
    {
        CharacterManager.i.SolveProblem(_selectedCharacter);
        TownGameManager.i.GoToRoom(_selectedCharacter);
    }

    public void ResetKitchen()
    {
        if (_spawnedCharacter) Destroy(_spawnedCharacter.gameObject);

        _subgameController.gameObject.SetActive(false);
        _backButton.SetActive(true);
        OpenCharacterSelect();
    }

    public void OpenCharacterSelect()
    {
        _characterSelectScreen.SetActive(true);
    }

    /// <summary>
    /// Called from the confirm window of the character selection menu
    /// </summary>
    public override void SelectPrimaryCharacter(ID id)
    {
        _selectedCharacter = id;
        _characterSelectScreen.SetActive(false);
        _recipientSelectScreen.gameObject.SetActive(true);

        _recipientSelectScreen.SelectPreviousPrimary(id);

        if (_areaController.SpawnedCharacter == null) _areaController.SpawnCharacter(id);
        _spawnedCharacter = _areaController.SpawnedCharacter;
        _spawnedCharacter.SetHat(_chefHat);
    }

    public override void SelectRecipient(ID id)
    {
        _selectedRecipient = id;
        _recipientSelectScreen.gameObject.SetActive(false);
        ShowRecipeOptions();
    }

    public void ShowRecipeOptions()
    {
        _recipeSelector.ShowRecipes(_recipes, _selectedCharacter, _selectedRecipient);
    }

    public void StartCooking(RecipeData recipe)
    {
        _backButton.SetActive(false);

        _recipeSelector.gameObject.SetActive(false);

        _subgameController.StartMinigame(recipe, _spawnedCharacter, _selectedRecipient, _solvingProblem);
    }

}
