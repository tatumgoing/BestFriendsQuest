using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager i;

    [SerializeField] private CutsceneDialogue _dialogueController;

    private Action _onCompleteCallback;

    public bool CurrentCutscene => _dialogueController.gameObject.activeInHierarchy;

    private void Awake()
    {
        i = this;
    }

    public void StartCutscene(TextAsset script, ID id1, Action callback = null) => StartCutscene(script, new List<ID>() { id1 }, callback);
    public void StartCutscene(TextAsset script, ID id1, ID id2, Action callback = null) => StartCutscene(script, new List<ID>() { id1, id2 }, callback);
    public void StartCutscene(TextAsset script, List<ID> characters, Action callback = null)
    {
        var names = new List<string>();
        foreach (var character in characters) names.Add(CharacterManager.i.GetNameFormatted(character));
        StartCutscene(script, names, callback);
    }

    public void StartCutscene(TextAsset script, List<string> names, Action callback = null)
    {
        _onCompleteCallback = callback;
        _dialogueController.StartDialogue(script.text.Split("\n").ToList(), names);
    }

    public void EndCutscene()
    {
        _onCompleteCallback?.Invoke();
    }
}
