using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkController : MonoBehaviour
{
    //[SerializeField] private List<Transform> _characterSpawns = new List<Transform>();
    [SerializeField] private List<SpawnedCharacter> _characters = new List<SpawnedCharacter>();


    /// <summary>
    /// rn TownGameManager just enables this object, so 'initialization' code is called from OnEnable
    /// </summary>
    private void OnEnable()
    {
        if (TownGameManager.i != null) Initialize();
    }

    private void Initialize()
    {
        var saveStrings = Utils.GetRandomCharacters(_characters.Count);
        for (int i = 0; i < _characters.Count; i++) {
            if (i >= saveStrings.Count) break;
            _characters[i].LoadFromString(saveStrings[i]);
        }
    }
}
