using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;

    public void Show(ID id)
    {
        var character = CharacterManager.i.GetCharacter(id);
        _nameText.text = character.Name + "'s";
        gameObject.SetActive(true);

    }
}
