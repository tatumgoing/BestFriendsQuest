using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField, ReadOnly] private List<int> _validIDs = new List<int>(); 
    [SerializeField] private int _toLoadID;
    [SerializeField] private CharacterMetaController _characterController;

    private List<string> _saveStrings = new List<string>();

    [ButtonMethod]
    public void RefreshIDList()
    {
        var savedText = File.ReadAllText(GameManager.i.CharactersSavePath);
        _saveStrings = savedText.Split('\n').Where(x => x.Length > 0).ToList();
        _validIDs = _saveStrings.Select(x => int.Parse(x.Substring(0, SaveSystem.IDLength))).ToList();
    }

    [ButtonMethod]
    public void LoadCharacter()
    {
        var saveString = GetSaveStringByID(_toLoadID);
        if (saveString != "")_characterController.LoadFromString(saveString);
    }

    private string GetSaveStringByID(int id) {
        var selected = _saveStrings.Where(x => int.Parse(x.Substring(0, SaveSystem.IDLength)) == id);
        if (selected.Count() == 0) {
            return "";
        }
        return selected.First();
    }
}
