using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParkController : MonoBehaviour
{
    //[SerializeField] private List<Transform> _characterSpawns = new List<Transform>();
    [SerializeField] private List<CharacterSpawnLocation> _spawnSpots = new List<CharacterSpawnLocation>();
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
    private async void Initialize()
    {
        foreach (var s in _spawnedCharacters) Destroy(s.gameObject);
        _spawnedCharacters.Clear();

        var IDs = CharacterManager.i.allCharacters.Select(x => x.ID).ToList();//.Shuffle();
        IDs.Reverse();
        print("IDs: " + string.Join(", ", IDs));

        IDs = IDs.Take(_spawnSpots.Count).ToList();
        _spawnSpots.Shuffle();

        for (int i = 0; i < IDs.Count; i++) {
            if (i >= _spawnSpots.Count) break;
            var newCharacter = CharacterManager.i.SpawnCharacter(IDs[i], _spawnSpots[i].transform);
            _spawnSpots[i].SetCharacter(newCharacter);
            _spawnedCharacters.Add(newCharacter);
        }

        foreach (var c in _spawnedCharacters) c.gameObject.SetActive(false);
        await System.Threading.Tasks.Task.Delay(100);

        for (int i = 0; i < _spawnedCharacters.Count; i++) {
            _spawnedCharacters[i].gameObject.SetActive(true);
            _spawnSpots[i].SetCharacter(_spawnedCharacters[i]);
        }
    }

    private void OnDisable()
    {
        foreach (var s in _spawnedCharacters) if (s) Destroy(s.gameObject);
        _spawnedCharacters.Clear();
    }
}
