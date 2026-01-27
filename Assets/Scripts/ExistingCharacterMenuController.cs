using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class ExistingCharacterMenuController : MonoBehaviour
{
    [SerializeField] private CharacterMetaController _characterController;
    [SerializeField] private CharacterCreatorProgression _progression;
    [SerializeField] private Transform _buttonListParent;
    [SerializeField] private MainHairController _mainHairController;
    [SerializeField] private ColorMenuController _skinColorController;

    [ReadOnly, SerializeField] private List<string> _saveStrings;

    private void OnEnable()
    {
        DisplayExistingCharacters();
    }

    private void DisplayExistingCharacters()
    {   
        if (!File.Exists(GameManager.i.Path) || string.IsNullOrEmpty(File.ReadAllText(GameManager.i.Path))) {
            _saveStrings.Clear();
            foreach (Transform child in _buttonListParent) {
                child.GetComponent<SelectableItem>().SetDisabled(true);
                child.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
            return;
        }

        var savedText = File.ReadAllText(GameManager.i.Path);
        _saveStrings = savedText.Split('\n').Where(x => x.Length > 0).ToList();
        var IDs = _saveStrings.Select(x => x.Substring(0, GameManager.idLength)).ToList();

        for (int i = 0; i < _saveStrings.Count; i++) {
            var button = _buttonListParent.GetChild(i);

            var profileData = new CharacterProfileData();
            profileData.FromString(_saveStrings[i].Split("|")[5]);

            button.GetComponentInChildren<TextMeshProUGUI>().text = profileData.Name;
        }
        foreach (Transform child in _buttonListParent) {
            var disabled = child.GetSiblingIndex() >= _saveStrings.Count;
            child.GetComponent<SelectableItem>().SetDisabled(disabled);
            if (disabled) child.GetComponentInChildren<TextMeshProUGUI>().text = "";

            child.GetComponent<SelectableItem>().OnSelect.RemoveAllListeners();
            child.GetComponent<SelectableItem>().OnSelect.AddListener(() => LoadCharacter(child.GetSiblingIndex()));
        }
    }

    [ButtonMethod]
    private void Load0() => LoadCharacter(0);

    public void LoadCharacter(int index)
    {
        _progression.StartNew();
        _skinColorController.Initialize();
        _characterController.LoadFromString(_saveStrings[index]);
        _mainHairController.SetHair(_saveStrings[index]);
        _skinColorController.SetColor(_characterController.SkinColor);

    }
}
