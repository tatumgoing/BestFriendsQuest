using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private CharacterStatusMenu _statusMenu;

    [Header("Speaking")]
    [SerializeField, TextArea(3, 10)] private List<string> _lines = new List<string>();
    [SerializeField] private GameObject _textBoxParent;
    [SerializeField] private TextMeshProUGUI _textBox;

    private ID _id;

    public void Show(ID id)
    {
        _id = id;
        var character = CharacterManager.i.GetCharacter(id);
        _nameText.text = character.Name + "'s";

        _statusMenu.gameObject.SetActive(false);
        _textBoxParent.SetActive(false);

        gameObject.SetActive(true);
    }

    public void ShowStatus()
    {
        _statusMenu.Show(_id);
    }

    public void Talk()
    {
        _textBox.text = _lines[Random.Range(0, _lines.Count)];
        _textBoxParent.SetActive(true);
    }
}
