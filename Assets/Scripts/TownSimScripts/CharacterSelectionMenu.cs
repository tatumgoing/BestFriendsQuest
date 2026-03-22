using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;
using UnityEngine.Events;
using MyBox;

public class CharacterSelectionMenu : MonoBehaviour
{
    [SerializeField] private MinigameController _minigameController;
    [SerializeField] private GameObject _characterButtonPrefab;
    [SerializeField] private Transform _listParent;
    [SerializeField] private CharacterSelectConfirmWindow _confirmWindow;
    [SerializeField] private GameObject _selectButton;
    [SerializeField] private bool _recipientSelector;
    [SerializeField, ConditionalField(nameof(_recipientSelector))] private TextMeshProUGUI _headerText;
    [SerializeField, ConditionalField(nameof(_recipientSelector))] private string _headerTemplateString = "Who is NAME cooking for?";

    [SerializeField] private UnityEvent<ID> _onSelect;

    private List<CharacterSelectButton> _spawnedButtons = new List<CharacterSelectButton>();
    private ID _selectedCharacter = new ID(0);
    private ID _alreadySelectedPrimary;

    //-------------------//
    [HideInInspector] public CompleteCharacterData selectedCharacter;
    //-------------------//

    public bool Recipient => _recipientSelector;

    public void SelectPreviousPrimary(ID id)
    {
        _alreadySelectedPrimary = id;
        if (_recipientSelector) {
            _headerText.text = _headerTemplateString.Replace("NAME", CharacterManager.i.GetNameFormatted(id));
        }
    }

    private void OnEnable()
    {
        _confirmWindow.gameObject.SetActive(false);
        _selectButton.SetActive(false);
        BuildSelectionList();
    }

    /// <summary>
    /// Creates the list of character buttons based on available characters from characterManager
    /// </summary>
    private void BuildSelectionList()
    {
        foreach (var b in _spawnedButtons) Destroy(b.gameObject);
        _spawnedButtons.Clear();

        foreach (var ID in CharacterManager.i.AllIDs()) SpawnCharacterButton(ID);
    }

    /// <summary>
    /// Based on the ID, spawns one character button and initializes it with the correct data, then adds it to the list of spawned buttons
    /// called from BuildSelectionList for each character ID in the character manager
    /// </summary>
    private void SpawnCharacterButton(ID id)
    {
        var newButton = Instantiate(_characterButtonPrefab, _listParent).GetComponent<CharacterSelectButton>();
        newButton.Initialize(id, this);
        _spawnedButtons.Add(newButton); 
    }

    /// <summary>
    /// Called from the character button when clicked
    /// </summary>
    public void SelectCharacter(ID id)
    {
        foreach (var button in _spawnedButtons) if (button.ID != id) button.Deselect();
        _selectedCharacter = id;
        _selectButton.SetActive(true);
    }

    /// <summary>
    /// Called from the button on the character selection menu to open the confirm window with the selected character's info, only opens if a character is selected (selectedCharacter != 0)
    /// </summary>
    public void ShowConfirmMenu()
    {
        if (_selectedCharacter == 0) return;

        if (_recipientSelector) _confirmWindow.Display(_alreadySelectedPrimary, _selectedCharacter, _minigameController);
        else _confirmWindow.Display(_selectedCharacter, _minigameController);

        _onSelect.Invoke(_selectedCharacter);
    }
}
