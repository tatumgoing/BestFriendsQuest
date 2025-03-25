using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class CharacterHouseButton : MonoBehaviour
{
    public TMP_Text labelText;
    public Image labelSprite;


    public void SetHouseLabel(string characterName)
    {
        labelText.text = characterName;
    }

    public void SetHouseSprite(Sprite characterSprite)
    {
        labelSprite.sprite = characterSprite;
    }
}
