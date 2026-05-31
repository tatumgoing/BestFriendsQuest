using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager i;

    [SerializeField] private CutsceneDialogue _dialogueController;

    public bool CurrentCutscene => _dialogueController.gameObject.activeInHierarchy;

    private void Awake()
    {
        i = this;
    }

    public void StartCutscene()
    {
        _dialogueController.StartDialogue();
    }
}
