using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum VignetteLocation { Home, Park }
[CreateAssetMenu(fileName = "Vignette", menuName = "Vignette", order = 4)]

public class Vignette : ScriptableObject
{
    public VignetteLocation Location;
    public int StartingCamIndex;

    [SerializeField] public List<DialogueT> VignetteDialogues = new List<DialogueT>();

}

[System.Serializable]
public class DialogueT  
{
    public string DialogueText;

    public int FocusedCharacter;

    public int CamIndex = -1;
}
