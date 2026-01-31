using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedCharacter : MonoBehaviour
{
    [SerializeField] private CharacterMetaController _characterController;

    public void LoadFromString(string saveString)
    {
        _characterController.LoadFromString(saveString);
    }
}
