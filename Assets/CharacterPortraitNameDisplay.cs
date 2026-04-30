using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPortraitNameDisplay : MonoBehaviour
{
    [SerializeField] private Image _portrait;
    [SerializeField] private TextMeshProUGUI _name;

    public ID ID { get; private set; }

    public void Clear()
    {
        _portrait.gameObject.SetActive(false);
        _name.text = "";
    }

    public void Show(ID id)
    {
        ID = id;
        
        _portrait.sprite = CharacterManager.i.GetPortrait(id);
        _portrait.gameObject.SetActive(true);

        _name.text = CharacterManager.i.GetNameFormatted(id);
    }
}
