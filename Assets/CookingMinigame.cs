using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameController : MonoBehaviour
{
    public virtual void StartMinigame(ID id) { }
}

public class CookingMinigame : MinigameController
{
    [SerializeField] private GameObject _characterSelectScreen;

    private void OnEnable()
    {
        OpenCharacterSelect();
    }

    private void OpenCharacterSelect()
    {
        print("Starting cooking!");
        _characterSelectScreen.SetActive(true);
    }

    /// <summary>
    /// Called from the confirm window of the character selection menu
    /// </summary>
    public override void StartMinigame(ID id)
    {
        base.StartMinigame(id);
        StartCooking(id);
    }

    public void StartCooking(ID id)
    {
        MinigameManager.i.NextMinigameScene();
    }
}
