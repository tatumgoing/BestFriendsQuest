using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrauntController : MonoBehaviour
{
    [SerializeField] private Transform _characterSpawnPoint;

    public GameObject SpawnCharacter(ID id)
    {
        var character = CharacterManager.i.SpawnCharacter(id, _characterSpawnPoint);
        character.transform.SetParent(_characterSpawnPoint);
        character.transform.localPosition = Vector3.zero;
        return character.gameObject;
    }
}
