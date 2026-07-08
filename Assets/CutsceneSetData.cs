using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CutsceneSetData
{
    [HideInInspector] public string DisplayName;

    public GameObject SetParent;
    public AreaName Area;
    public Transform Camera;
    public Sound music;

    [SerializeField] private List<Transform> _spawnPoints = new List<Transform>();

    private List<SpawnedCharacter> _spawnedCharacters = new List<SpawnedCharacter>();

    public void ClearCharacters()
    {
        foreach (var c in _spawnedCharacters) if (c) GameObject.Destroy(c.gameObject);
        _spawnedCharacters.Clear();
    }

    public List<SpawnedCharacter> SpawnCharacters(List<ID> ids, bool startMusic = true)
    {
        ClearCharacters();
        var characters = new List<SpawnedCharacter>();

        for (int i = 0; i < _spawnPoints.Count; i++) {
            if (i >= ids.Count) break;
            var c = CharacterManager.i.SpawnCharacter(ids[i], _spawnPoints[i]);
            _spawnedCharacters.Add(c);
            characters.Add(c);
            //Debug.Log("Spawned character: " + CharacterManager.i.GetNameFormatted(ids[i]));
        }

        if (startMusic) StartMusic();

        return characters;
    }

    private void StartMusic()
    {
        if (!music.Instantialized) music = GameObject.Instantiate(music);

        if (music == null) {
            TownMusicPlayer.i.StopCurrentTrack();
        }

        else if (TownMusicPlayer.i.currentTrack == null) {
            TownMusicPlayer.i.PlayNewTrack(music);
        }
        else if (TownMusicPlayer.i.currentTrack.TrackName != music.TrackName) {
            TownMusicPlayer.i.PlayNewTrack(music);
        }

    }
}
