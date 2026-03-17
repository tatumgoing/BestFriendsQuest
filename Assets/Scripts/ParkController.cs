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

    private void OnDisable() => ClearAllCharacters();

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
        ClearAllCharacters();
        SpawnAllCharacters();
    }

    /// <summary>
    /// Deletes all spawned characters and resets the list.
    /// </summary>
    private void ClearAllCharacters()
    {
        foreach (var s in _spawnedCharacters) Destroy(s.gameObject);
        _spawnedCharacters.Clear();
    }

    /// <summary>
    /// Spawns the most recent x number of characters, where x is the number of spawn spots. 
    /// (if there are more spawn spots than characters, spawns all characters and leaves some spawn spots empty)
    /// </summary>
    private void SpawnAllCharacters()
    {
        var IDs = SelectCharactersToSpawn();

        _spawnSpots.Shuffle();
        for (int i = 0; i < Mathf.Min(IDs.Count, _spawnSpots.Count); i++) {
            SpawnCharacter(IDs[i], _spawnSpots[i]);
        }
    }

    /// <summary>
    /// Spawns 1 character and sets its animation. enables and  disables the spawned character quickly to force update the rig.
    /// </summary>
    private async void SpawnCharacter(ID id, CharacterSpawnLocation spawnPoint)
    {
        var newCharacter = CharacterManager.i.SpawnCharacter(id, spawnPoint.transform);
        
        //newCharacter.gameObject.SetActive(false);
        //await System.Threading.Tasks.Task.Delay(100);
        //newCharacter.gameObject.SetActive(true);
        
        spawnPoint.SetCharacter(newCharacter);

        _spawnedCharacters.Add(newCharacter);
    }

    /// <summary>
    /// Selects which characters should spawn (currently just selects the most recent x number of characters, 
    /// where x is the number of spawn spots. if there are more spawn spots than characters, 
    /// selects all characters and leaves some spawn spots empty)
    /// </summary>
    private List<ID> SelectCharactersToSpawn()
    {
        var IDs = CharacterManager.i.allCharacters.Select(x => x.ID).ToList();
        IDs.Reverse();

        IDs = IDs.Take(_spawnSpots.Count).ToList();
        return IDs;
    }

}
