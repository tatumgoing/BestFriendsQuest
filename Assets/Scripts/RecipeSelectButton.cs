using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SelectableItem))]
public class RecipeSelectButton : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _nameText;

    private SelectableItem _button;
    private RecipeSelector _controller;
    private RecipeData _recipe;

    public RecipeData Recipe => _recipe;

    public void Initialize(RecipeData recipe, RecipeSelector controller)
    {
        _nameText.text = recipe.Name;
        _icon.sprite = recipe.Icon;

        _button = GetComponent<SelectableItem>();   
        _recipe = recipe;
        _controller = controller;
    }

    public void Select()
    {
        _controller.Select(_recipe);
    }

    public void Deselect()
    {
        _button.Deselect(true, false);
    }
}
