using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborhoodController : MonoBehaviour
{
    private void OnEnable()
    {
        HouseCharacters();
    }

    private void HouseCharacters()
    {
        var houses = GetComponentsInChildren<HouseController>();
        houses.Shuffle();

        var characters = CharacterManager.i.AllIDs();

        for (int i = 0; i < houses.Length; i++) {
            if (i >= characters.Count) houses[i].Hide();
            else houses[i].Initialize(characters[i]);
        }
    }
}
