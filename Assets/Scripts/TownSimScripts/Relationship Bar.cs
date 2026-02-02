using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RelationshipBar : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Slider _slider;

    private ID _id1;
    private ID _id2;

    void Update()
    {
        var relationshipValue = CharacterManager.i.GetRelationship(_id1, _id2);
        _text.text = "Relationship Level: " + Mathf.Floor(relationshipValue).ToString();
        _slider.value = relationshipValue - Mathf.Floor(relationshipValue);
    }

    public void SetCharacters(ID id1, ID id2)
    {
        _id1 = id1;
        _id2 = id2;
    }
}
