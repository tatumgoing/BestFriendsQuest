using UnityEngine;

public abstract class MinigameController : MonoBehaviour
{
    public abstract MinigameType GetMinigameType();
    public abstract void SelectPrimaryCharacter(ID id);
    public abstract void SelectRecipient(ID id);
    public abstract void StartProblemMinigame(ID character);
    public abstract void CompleteProblem();
    public abstract Transform GetCamera();
}
