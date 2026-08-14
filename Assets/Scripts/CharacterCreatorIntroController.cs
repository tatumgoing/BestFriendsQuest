using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterCreatorIntroController : MonoBehaviour
{
    [SerializeField] private SelectableItem _editExistingButton;

    private void OnEnable()
    {
        var characterStrings = SaveSystem.LoadAllStaticSaveStrings().Where(x => x.Length > 3).ToList();
        _editExistingButton.SetDisabled(characterStrings.Count == 0);
    }
}
