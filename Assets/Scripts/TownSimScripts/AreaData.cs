using MyBox;
using UnityEngine;

public enum AreaName { MAP, PARK, TOWN, SHOP, RESTURAUNT, TOWN_HALL, PORT, RECORDS}

[System.Serializable]
public class AreaData
{
    [HideInInspector] public string DisplayName;
    [HideInInspector] public AreaName Type;

    public GameObject UI;
    public GameObject Environment;
    [SerializeField] private bool _hasMinigame;
    [ConditionalField(nameof(_hasMinigame))] public MinigameController _minigameController;

    public void Show() => SetActiveState(true);
    public void Hide() => SetActiveState(false);

    public void SetActiveState(bool active)
    {
        if (UI) UI.SetActive(active);
        if (Environment) Environment.SetActive(active);
    }
}
