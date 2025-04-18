using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappinessBar : MonoBehaviour
{
    public CharacterData associatedCharacter;

    public GameObject happinessBackground;
    public GameObject happinessProgress;
    void Update()
    {
        UpdateMeter();
    }
    void UpdateMeter()
    {
        float newWidth = happinessBackground.GetComponent<RectTransform>().sizeDelta.x * (associatedCharacter.happiness / 100);
        happinessProgress.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, happinessProgress.GetComponent<RectTransform>().sizeDelta.y);

    }
}
