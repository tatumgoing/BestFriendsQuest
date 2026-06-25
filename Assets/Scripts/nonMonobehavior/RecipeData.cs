using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MinigameType { COOKING, GARDENING, FISHING}


public enum Difficulty { EASY, MEDIUM, HARD, EXTREME }

[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe", order = 1)]

public class RecipeData : ScriptableObject
{
    public string Name;
    [TextArea(3,10)] public string Description;
    public Sprite Icon;


    [Header("Rewards")]
    public Difficulty Difficulty;
    [SerializeField] private bool _overrideRewards;
    public float MaxScore;
    public int MoneyReward;
    public float HappinessReward = 30;
    public float RelationshipReward = 1.5f;


    public List<SubgameData> Subgames = new List<SubgameData>();

    // enum to verb pls
    static Dictionary<SubgameType, string> subgameToVerb =
    new() {
        {SubgameType.STIRRING, "Stir"},
        {SubgameType.GRILLING, "Grill"},
        {SubgameType.CHOPPING, "Chop"},
        {SubgameType.BOILING, "Boil"},
        {SubgameType.STEAMING, "Steam"},
        {SubgameType.ROLLING, "Roll"},


    };

    private void OnValidate()
    {
        if (_overrideRewards) return;

        var difficulty = ((int)Difficulty + 1);
        MaxScore = difficulty * 20;
        MoneyReward = difficulty * 50;
        HappinessReward = difficulty * 60;
        RelationshipReward = difficulty * 0.45f + 0.2f;
    }

    public string ReturnSteps()
    {
        string stepsStr = "";

        //for every subgame, add the number of step, the verb, and a line break
        for (int i = 0; i < Subgames.Count; i++) {
            stepsStr += (i+1) + ". " + subgameToVerb[Subgames[i].Type] + " " + Subgames[i].IngredientName + "\n";
        }

        return stepsStr;
    }


}
