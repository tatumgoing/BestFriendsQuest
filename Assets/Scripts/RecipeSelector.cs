using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeSelector : MonoBehaviour
{
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private Transform _listParent;
    [SerializeField] private GameObject _startButton;
    [SerializeField] private CookingMinigame _controller;
    [SerializeField] private RecipeDisplay _currentRecipe;


    private List<RecipeSelectButton> _spawnedButtons = new List<RecipeSelectButton>();
    private RecipeData _selected;

    public void ShowRecipes(List<RecipeData> selectedRecipes)
    {
        _startButton.SetActive(false);
        BuildRecipeList(selectedRecipes);
        gameObject.SetActive(true);
    }

    private void BuildRecipeList(List<RecipeData> selectedRecipes)
    { 
        foreach (var s in _spawnedButtons) Destroy(s.gameObject);   
        _spawnedButtons.Clear();

        foreach (var r in selectedRecipes) SpawnRecipe(r);

        if (_spawnedButtons.Count > 0) Select(_spawnedButtons[0].Recipe);
    }

    private void SpawnRecipe(RecipeData recipe)
    {
        var newButton = Instantiate(_buttonPrefab, _listParent).GetComponent<RecipeSelectButton>();
        newButton.Initialize(recipe, this);
        _spawnedButtons.Add(newButton);
    }

    public void Select(RecipeData recipe)
    {
        foreach (var s in _spawnedButtons) if (s.Recipe != recipe) s.Deselect();
        _selected = recipe;
        UpdateSelectDisplay();

        _startButton.SetActive(true);   
    }

    public void UpdateSelectDisplay()
    {
        var highscore = 0f;
        Dictionary<string, float> tempDict = SaveSystem.LoadHighscoreDictionary("Cooking");
        if (tempDict.ContainsKey(_selected.name)) {
            highscore = tempDict[_selected.name];
        }

        _currentRecipe.Show(_selected, highscore);
    }


    public void StartCooking()
    {
        _controller.StartCooking(_selected);
        gameObject.SetActive(false);
    }
}
