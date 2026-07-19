using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CutsceneButton : MonoBehaviour
{
    [SerializeField] private List<CutsceneScript> _scripts = new List<CutsceneScript>();
    private List<CutsceneScript> _usedConvos = new List<CutsceneScript>();
    [SerializeField] private GameObject _cam;

    private SpawnedCharacter _speaker1;
    private SpawnedCharacter _speaker2;

    private void OnValidate()
    {
        foreach (var s in _scripts) s.OnValidate();
    }

    public void SetCharacters(SpawnedCharacter speaker1, SpawnedCharacter speaker2)
    {
        _speaker1 = speaker1;
        _speaker2 = speaker2;
    }

    public void SetCharacters(SpawnedCharacter speaker1)
    {
        _speaker1 = speaker1;
        _speaker2 = null;
    }

    public void StartCutscene()
    {
        var validScripts = new List<CutsceneScript>(_scripts);
        if (_speaker2 == null) validScripts = validScripts.Where(x => x.Monologue).ToList();
        else validScripts = validScripts.Where(x => !x.Monologue).ToList();
        
        ID ID2 = _speaker2 != null ? _speaker2.ID : new ID();
        validScripts = validScripts.Where(x => x.Check(_speaker1.ID, ID2)).ToList();

        var selected = Random.Range(0, validScripts.Count);
        CutsceneManager.i.StartCutscene(validScripts[selected].Script, _cam.transform, _speaker1, _speaker2, ResetCam);

        _usedConvos.Add(validScripts[selected]);
        _scripts.RemoveAt(selected);

        if (_scripts.Count == 0) {
            _scripts = new List<CutsceneScript>(_usedConvos);
            _usedConvos.Clear();
            _scripts.Shuffle();
        }

        _cam.SetActive(true);
        _cam.transform.parent = null;

        gameObject.SetActive(false);
    }

    public void ResetCam()
    {
        _cam.SetActive(false);
        _cam.transform.SetParent(transform);
    }
}
