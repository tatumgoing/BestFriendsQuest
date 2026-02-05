using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterRoomModel : MonoBehaviour
{
    [SerializeField] private Transform _characterSpawnSpot;

    private GameObject _spawnedCharacter;

    public void Show(ID id)
    {
        transform.parent.gameObject.SetActive(true);
        gameObject.SetActive(true);
        SpawnCharacter(id);
    }

    public void SpawnCharacter(ID character)
    {
        if (_spawnedCharacter != null) Destroy(_spawnedCharacter);
        _spawnedCharacter = CharacterManager.i.SpawnCharacter(character, _characterSpawnSpot).gameObject;
    }
    
    private void OnDisable()
    {
        if (_spawnedCharacter != null) Destroy(_spawnedCharacter);
    }
}
