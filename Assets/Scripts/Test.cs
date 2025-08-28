using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private CharacterMetaController _character;
    [SerializeField, TextArea(3, 10)] private string _output;
    [SerializeField, TextArea(3, 10)] private string _input;

    [ButtonMethod]
    private void GetSaveString()
    {
        _output = _character.GetSaveString();
    }

    [ButtonMethod]
    private void LoadString()
    {
        _character.LoadFromString(_input);
    }

    private void OnDisable()
    {
        //print("Disabled");
    }
}
