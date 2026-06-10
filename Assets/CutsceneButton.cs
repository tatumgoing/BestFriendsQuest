using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CutsceneButton : MonoBehaviour
{
    [SerializeField] private List<TextAsset> _scripts = new List<TextAsset>();
    private List<TextAsset> _usedConvos = new List<TextAsset>();
    [SerializeField] private GameObject _cam;

    private SpawnedCharacter _speaker1;
    private SpawnedCharacter _speaker2;

    private string _name1 => CharacterManager.i.GetNameFormatted(_speaker1.ID);
    private string _name2 => _speaker2 ? CharacterManager.i.GetNameFormatted(_speaker2.ID) : "";

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
        var validScripts = new List<TextAsset>(_scripts);
        if (_name2 == "") validScripts = validScripts.Where(x => !x.text.Contains("c2")).ToList();
        else validScripts = validScripts.Where(x => x.text.Contains("c2")).ToList();

        var selected = Random.Range(0, validScripts.Count);
        CutsceneManager.i.StartCutscene(validScripts[selected], _cam.transform, _speaker1, _speaker2, ResetCam);

        _usedConvos.Add(validScripts[selected]);
        _scripts.RemoveAt(selected);

        if (_scripts.Count == 0) {
            _scripts = new List<TextAsset>(_usedConvos);
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
