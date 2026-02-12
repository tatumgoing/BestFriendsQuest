using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class SpawnedCharacter : MonoBehaviour
{
    [SerializeField] private CharacterMetaController _characterController;
    [SerializeField] public ID ID;

    public void LoadFromString(string saveString)
    {
        _characterController.LoadFromString(saveString);
        this.ID = _characterController.Data.ID;

        gameObject.name = _characterController.Data.Name + " (spawned character)";
    }
}
