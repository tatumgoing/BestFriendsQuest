using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterRoomModel : MonoBehaviour
{
    [SerializeField] private Transform _characterSpawnSpot;

    public void Show(ID id)
    {
        gameObject.SetActive(true);
        SpawnCharacter(id);
    }

    public void SpawnCharacter(ID character)
    {
        CharacterManager.i.SpawnCharacter(character, _characterSpawnSpot);
    }
}
