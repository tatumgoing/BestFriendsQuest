using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParkController : MonoBehaviour
{
    //[SerializeField] private List<Transform> _characterSpawns = new List<Transform>();
    [SerializeField] private List<Transform> _spawnSpots = new List<Transform>();
    private List<SpawnedCharacter> _spawnedCharacters = new List<SpawnedCharacter>();

    /// <summary>
    /// rn TownGameManager just enables this object, so 'initialization' code is called from OnEnable
    /// </summary>
    private void OnEnable()
    {
        if (TownGameManager.i != null) Initialize();
    }

    /// <summary>
    /// On startup, delete any existing spawned characters and spawn new ones.
    /// spawn as many characters as there are spawn spots (or as many as there are characters if fewer)
    /// </summary>
    private void Initialize()
    {
        foreach (var s in _spawnedCharacters) Destroy(s.gameObject);
        _spawnedCharacters.Clear();

        var IDs = CharacterManager.i.allCharacters.Select(x => x.ID).ToList().Shuffle();
        IDs.Take(_spawnSpots.Count).ToList();
        _spawnSpots.Shuffle();

        for (int i = 0; i < IDs.Count; i++) {
            if (i >= _spawnSpots.Count) break;
            var newCharacter = CharacterManager.i.SpawnCharacter(IDs[i], _spawnSpots[i]);
            _spawnedCharacters.Add(newCharacter);
        }
    }

    private void OnDisable()
    {
        foreach (var s in _spawnedCharacters) if (s) Destroy(s.gameObject);
        _spawnedCharacters.Clear();
    }
}
