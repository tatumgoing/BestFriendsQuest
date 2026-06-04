using UnityEngine;
using UnityEngine.UI;

public class CharacterToken :MonoBehaviour
{
    [SerializeField] private Image _portrait;

    public ID ID { get; private set;  }

    public void Initialize(ID id)
    {
        ID = id;
        _portrait.sprite = CharacterManager.i.GetPortrait(id);
    }
}
