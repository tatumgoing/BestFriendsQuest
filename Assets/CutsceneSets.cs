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

    [SerializeField] private List<Transform> _spawnPoints = new List<Transform>();

    private List<SpawnedCharacter> _spawnedCharacters = new List<SpawnedCharacter>();

    public void ClearCharacters()
    {
        foreach (var c in _spawnedCharacters) if (c) GameObject.Destroy(c.gameObject);
        _spawnedCharacters.Clear();
    }

    public List<SpawnedCharacter> SpawnCharacters(List<ID> ids)
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

        return characters;
    }
}

public class CutsceneSets : MonoBehaviour
{
    [SerializeField] private List<CutsceneSetData> _sets = new List<CutsceneSetData>();

    public CutsceneSetData Current { get; private set; }

    private void OnValidate()
    {
        foreach (var s in _sets) s.DisplayName = s.Area.ToString();
    }

    private void Start()
    {
        HideAll();
    }

    public void HideAll()
    {
        foreach (var s in _sets) {
            s.ClearCharacters();
            s.SetParent.SetActive(false);
        }
    }

    public List<SpawnedCharacter> ShowSet(AreaName area, List<ID> ids)
    {
        HideAll();

        var characters = new List<SpawnedCharacter>();
        foreach (var s in _sets) {
            if (s.Area != area) {
                s.SetParent.SetActive(false);
                continue;
            }

            s.SetParent.SetActive(true);
            Current = s;
            characters = s.SpawnCharacters(ids);
        }

        return characters;
    }
}
