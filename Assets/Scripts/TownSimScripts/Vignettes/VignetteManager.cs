using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

public class VignetteManager : MonoBehaviour
{
    bool isPlaying= false; 
    public VignetteUI vignetteUI;

    Vignette currentVignette;
    DialogueT currentDialogue;

    int currentDialogueIndex;

    List<CinemachineVirtualCamera> cameras= new List<CinemachineVirtualCamera>();
    bool textScrollComplete=false;

    [Header("Text Controls")]

    public float textSpeed;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isPlaying)
        {
            VignetteClicked();
        }

    }
    public void StartVignette(Vignette vignetteImport, List<CinemachineVirtualCamera> cameraImport)
    {
        vignetteUI.ShowTextBox();
        isPlaying = true;

        currentVignette = vignetteImport;
        currentDialogue = currentVignette.VignetteDialogues[currentDialogueIndex];

        cameras.AddRange(cameraImport);

        vignetteUI.ClearText();
        DisplayText();

    }

    void DisplayText()
    {
        Debug.Log("Displaying");

        StartCoroutine(BuildText());
    }

    private IEnumerator BuildText()
    {

        for (int i = 0; i < currentDialogue.DialogueText.Length; i++)
        {
            if (textScrollComplete)
            {
                break;
            }
            else
            {
                vignetteUI.ChangeText(vignetteUI.dialogueText.text += currentDialogue.DialogueText[i]);
            }

            yield return new WaitForSeconds(textSpeed);
        }

        textScrollComplete = true;
        
       
    }


    void VignetteClicked()
    {
        //if the text is all showing, check for next dialogue. if more dialogue, display. if not, end vignette
        if (textScrollComplete)
        {
            currentDialogueIndex++;

            if (currentDialogueIndex < currentVignette.VignetteDialogues.Count)
            {
                NextDialogue();
            }
            else
            {
                EndVignette();
            }
        }
        else
        {
            SkipDialogue();
        }
    }
    void NextDialogue()
    {
        currentDialogue = currentVignette.VignetteDialogues[currentDialogueIndex];

        textScrollComplete = false;
        vignetteUI.ClearText();
        DisplayText();

    }

    void SkipDialogue()
    {
        StopAllCoroutines();
        vignetteUI.ChangeText(vignetteUI.dialogueText.text = currentDialogue.DialogueText);
        textScrollComplete = true;
    }

    public void EndVignette()
    {
        currentVignette = null;
        vignetteUI.ClearText();
        vignetteUI.HideTextBox();

        currentDialogueIndex = 0;

        textScrollComplete= false;
        isPlaying = false;
    }
}
