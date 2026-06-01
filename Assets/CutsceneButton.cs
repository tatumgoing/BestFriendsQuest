using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneButton : MonoBehaviour
{
    [SerializeField] private List<TextAsset> _scripts = new List<TextAsset>();
    private List<TextAsset> _usedConvos = new List<TextAsset>();
    [SerializeField] private GameObject _cam;

    private string _name1;
    private string _name2;

    public void SetCharacterNames(string name1, string name2)
    {
        _name1 = name1;
        _name2 = name2;
    }

    public void StartCutscene()
    {
        var selected = Random.Range(0, _scripts.Count);
        CutsceneManager.i.StartCutscene(_scripts[selected], new List<string> { _name1, _name2 }, this);

        _usedConvos.Add(_scripts[selected]);
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
