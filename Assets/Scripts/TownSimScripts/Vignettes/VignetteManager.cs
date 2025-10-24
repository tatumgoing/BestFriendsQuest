using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

public class VignetteManager : MonoBehaviour
{
    public VignetteCameras cameraManager;

    public bool isPlaying= false; 
    public VignetteUI vignetteUI;

    Vignette currentVignette;
    DialogueT currentDialogue;

    int currentDialogueIndex;

    bool textScrollComplete=false;

    //Camera Controls

    CinemachineVirtualCamera currentCamera;
    List<CinemachineVirtualCamera> currentVignetteCameras = new List<CinemachineVirtualCamera>();

    [Header("Text Controls")]

    public float textSpeed;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isPlaying)
        {
            VignetteClicked();

        }

    }
    public void StartVignette(Vignette vignetteImport)
    {
        vignetteUI.ShowTextBox();
        isPlaying = true;

        currentVignetteCameras = cameraManager.vignetteCameras[vignetteImport.Location.ToString()];

        Debug.Log(vignetteImport.Location.ToString());

        currentVignette = vignetteImport;
        currentDialogue = currentVignette.VignetteDialogues[currentDialogueIndex];

        vignetteUI.ClearText();
        DisplayText();
        StartCamera();
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
        
        ChangeCamera();
        DisplayText();
    }

    void StartCamera()
    {
        for (int i = 0; i < currentVignetteCameras.Count; i++)
        {

            if (currentVignette.StartingCamIndex == i)
            {
                currentVignetteCameras[i].Priority = 100;
            }
            else
            {
                currentVignetteCameras[i].Priority = 0;
            }

        }
    }
    void ChangeCamera()
    {

        //if the cam index is not -1, change the camera
        if (currentDialogue.CamIndex >= 0)
        {

            for (int i = 0; i < currentVignetteCameras.Count; i++)
            {

                if (currentDialogue.CamIndex == i)
                {
                    currentVignetteCameras[i].Priority = 100;
                }
                else
                {
                    currentVignetteCameras[i].Priority = 0;
                }


            }
        }


    }

    void SkipDialogue()
    {
        StopAllCoroutines();
        vignetteUI.ChangeText(vignetteUI.dialogueText.text = currentDialogue.DialogueText);
        textScrollComplete = true;
    }

    public void EndVignette()
    {
        //set priorities back to 0 for vignette cameras
        foreach (CinemachineVirtualCamera v in currentVignetteCameras) { 
        
            v.Priority = 0;

        }

        //reset manager
        ResetVignetteManager();
    }

    public void ResetVignetteManager()
    {
        currentVignette = null;
        vignetteUI.ClearText();
        vignetteUI.HideTextBox();

        currentVignetteCameras = new List<CinemachineVirtualCamera>();

        currentDialogueIndex = 0;

        textScrollComplete = false;
        isPlaying = false;
    }
}
