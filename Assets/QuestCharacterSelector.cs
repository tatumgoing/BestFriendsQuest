using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class QuestCharacterSelector : MonoBehaviour
{
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private Transform _gridParent;
    [SerializeField] private GameObject _confirmButton;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _recommendedText;
    [SerializeField] private TextMeshProUGUI _relationshipLevelText;
    [SerializeField] private GameObject _rightTextParent;

    private List<QuestCharacterButton> _spawnedButtons = new List<QuestCharacterButton>();
    private Action<ID> _callBack;
    private ID _selected;
    private ID _alreadySelected = null;
    private float _recommendedLevel;

    public void Show(Action<ID> callBack, ID alreadySelected, float recommendedLevel)
    {
        _alreadySelected = alreadySelected;
        _recommendedLevel = recommendedLevel;

        _recommendedText.text = _relationshipLevelText.text = "";
        _rightTextParent.SetActive(true);

        Show(callBack, false);
    }

    public void Show(Action<ID> callBack, bool hideRightText = true)
    {
        if (hideRightText) {
            _rightTextParent.SetActive(false);
            _alreadySelected = null;
        }

        _nameText.text = "";
        _callBack = callBack;

        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        _confirmButton.SetActive(false);
        BuildList();
    }

    private void BuildList()
    {
        foreach (var button in _spawnedButtons) Destroy(button.gameObject);
        _spawnedButtons.Clear();

        var characters = CharacterManager.i.AllIDs();

        if (_rightTextParent.activeInHierarchy) {
            characters = characters.Where(x => x!= _alreadySelected).ToList();
            characters.Sort(
                (x, y) => 
                    CharacterManager.i.GetRelationship(_alreadySelected, y).CompareTo(CharacterManager.i.GetRelationship(_alreadySelected, x))
            );
        }

        foreach (var c in characters) {
            var b = Instantiate(_buttonPrefab, _gridParent).GetComponent<QuestCharacterButton>();
            b.Initialize(c, this);
            _spawnedButtons.Add(b);
        }
    }

    public void Select(ID id)
    {
        if (_rightTextParent.activeInHierarchy) {
            var value = CharacterManager.i.GetRelationship(id, _alreadySelected);
            _relationshipLevelText.text = "Relationship to " + CharacterManager.i.GetNameFormatted(_alreadySelected) + ": " + (Mathf.Floor(value));
            _recommendedText.text = "Recommended Level: " + (Mathf.Floor(_recommendedLevel));
        }

        _nameText.text = CharacterManager.i.GetNameFormatted(id);
        _selected = id;
        _confirmButton.SetActive(true);
        foreach (var b in _spawnedButtons) if (id != b.ID) b.Deselect();
    }

    public void Confirm()
    {
        _callBack.Invoke(_selected);
        gameObject.SetActive(false);
    }
}
