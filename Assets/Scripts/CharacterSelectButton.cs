using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SelectableItem))]
public class CharacterSelectButton : MonoBehaviour
{
    [SerializeField] private Image _portrait;
    [SerializeField] private HappinessBar _happiness;

    private CharacterSelectionMenu _controller;
    private ID _id;

    public ID ID => _id;

    /// <summary>
    /// Called from the controller, initializes this button's data
    /// </summary>
    public void Initialize(ID id, CharacterSelectionMenu controller)
    {
        _id = id;
        _controller = controller;
        _portrait.sprite = CharacterManager.i.GetPortrait(id);
        if (_happiness) _happiness.Initialize(id);
    }

    /// <summary>
    /// Called from the button script on this object
    /// </summary>
    public void Select()
    {
        _controller.SelectCharacter(_id);
    }

    /// <summary>
    /// Called from the controller to toggle off all the non-selected buttons
    /// </summary>
    public void Deselect()
    {
        GetComponent<SelectableItem>().Deselect(true, false);
    }
}
