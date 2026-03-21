using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingMinigame : MinigameController
{
    [SerializeField] private GameObject _characterSelectScreen;
    [SerializeField] private RecipeSelector _recipeSelector;
    [SerializeField] private RestrauntController _areaController;

    //TESTING
    [SerializeField] private List<RecipeData> _recipes;

    private ID selectedCharacter = new ID(0);

    private void OnEnable()
    {
        _recipeSelector.gameObject.SetActive(false);

        if (CharacterManager.i) OpenCharacterSelect();
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

        selectedCharacter = id;
        _characterSelectScreen.SetActive(false);
        ShowRecipeOptions();
    }

    public void ShowRecipeOptions()
    {
        _recipeSelector.ShowRecipes(_recipes);
    }

    public void StartCooking(RecipeData recipe)
    {
        _areaController.SpawnCharacter(selectedCharacter);
        //MinigameManager.i.NextMinigameScene();
    }
}
