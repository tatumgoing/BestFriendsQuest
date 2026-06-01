using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager i;

    [SerializeField] private CutsceneDialogue _dialogueController;

    private CutsceneButton _source;

    public bool CurrentCutscene => _dialogueController.gameObject.activeInHierarchy;

    private void Awake()
    {
        i = this;
    }

    public void StartCutscene(TextAsset script, List<string> names, CutsceneButton source)
    {
        _source = source;
        _dialogueController.StartDialogue(script.text.Split("\n").ToList(), names);
    }

    public void EndCutscene()
    {
        _source.ResetCam();
    }
}
