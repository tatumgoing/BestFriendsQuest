using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public bool IsActive()
    {
        foreach (var s in _sets) if (s.SetParent.activeInHierarchy) return true;
        return false;
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
