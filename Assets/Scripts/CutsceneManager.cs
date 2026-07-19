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

    public void StartCutscene(TextAsset script, Transform camera, SpawnedCharacter speaker1, SpawnedCharacter speaker2 = null, Action callback = null)
    {
        var names = new List<string>();
        names.Add(CharacterManager.i.GetNameFormatted(speaker1.ID));
        if (speaker2 != null) names.Add(CharacterManager.i.GetNameFormatted(speaker2.ID));
        _onCompleteCallback = callback;
        _dialogueController.StartDialogue(script.text.Split("\n").ToList(), names, speaker1, speaker2, camera);
    }

    public void EndCutscene()
    {
        _onCompleteCallback?.Invoke();
    }
}
