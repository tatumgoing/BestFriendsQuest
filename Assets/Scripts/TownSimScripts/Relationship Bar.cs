using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RelationshipBar : MonoBehaviour
{
    public CharacterData associatedCharacter;
    public CharacterData secondCharacter;

    public GameObject relationshipBackground;
    public GameObject relationshipProgress;

    public TMP_Text relationshipLevel;
    void Update()
    {
        if(associatedCharacter != null)
        {
            UpdateMeter();
            UpdateText();
        }
    }
    void UpdateMeter()
    {

        float newWidth = relationshipBackground.GetComponent<RectTransform>().sizeDelta.x * (associatedCharacter.relationships[secondCharacter]%1 / 1);
        relationshipProgress.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, relationshipProgress.GetComponent<RectTransform>().sizeDelta.y);

    }

    void UpdateText()
    {
        relationshipLevel.text = "Relationship Level: " + Mathf.Floor(associatedCharacter.relationships[secondCharacter]).ToString();
    }
}
