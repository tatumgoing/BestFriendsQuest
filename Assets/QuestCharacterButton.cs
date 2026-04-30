using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestCharacterButton : MonoBehaviour
{
    [SerializeField] private Image _sprite;

    private SelectableItem _button;
    private ID _id;
    private QuestCharacterSelector _controller;

    public ID ID => _id;

    private void Awake()
    {
        _button = GetComponent<SelectableItem>();
    }

    public void Initialize(ID id, QuestCharacterSelector controller)
    {
        _id = id;
        _controller = controller;
        _sprite.sprite = CharacterManager.i.GetPortrait(id);

        _button.SetDisabled(CharacterManager.i.GetHappiness(id) < 100);
    }

    public void Select()
    {
        _controller.Select(_id);
    }

    public void Deselect() => _button.Deselect(true, false);
}
