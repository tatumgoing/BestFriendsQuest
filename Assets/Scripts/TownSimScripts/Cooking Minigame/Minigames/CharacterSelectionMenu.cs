using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionMenu : MonoBehaviour
{
    public MinigameManager manager;
    // Start is called before the first frame update
    
    void Start()
    {
        manager.GenerateCharacterSelect();
    }
    void OnEnable()
    {
        manager.GenerateCharacterSelect();
    }
}
