using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IdPhotoController : MonoBehaviour
{
    [SerializeField] private Image _photoImg;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _birthdayText;

    [SerializeField] private CharacterMetaController _character;

    public void ShowPicture(CompleteCharacterData character)
    {
        _photoImg.sprite = character.Icon;
        _nameText.text = character.Name;

        var birthday = character.Birthday;
        _birthdayText.text = birthday.Month + " / " + birthday.Day + " / " + birthday.Year;
    }

    public void ShowPicture(Texture2D idTexture)
    {
        var idSprite = Sprite.Create(
           idTexture,
           new Rect(0, 0, idTexture.width, idTexture.height),
           new Vector2(0.5f, 0.5f)
        );

        _photoImg.sprite = idSprite;
        gameObject.SetActive(true);

        _nameText.text = _character.Data.Name;
        var birthday = _character.Data.Birthday;
        _birthdayText.text = birthday.Month + " / " + birthday.Day + " / " + birthday.Year;
    }
}
