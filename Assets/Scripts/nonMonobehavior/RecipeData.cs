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
    public float MaxScore;
    public int MoneyReward;
    public float HappinessReward = 30;
    public float RelationshipReward = 1.5f;

    public Difficulty Difficulty;

    public List<SubgameData> Subgames = new List<SubgameData>();

    // enum to verb pls
    static Dictionary<SubgameType, string> subgameToVerb =
    new Dictionary<SubgameType, string>() {
        {SubgameType.SITRRING, "Stir"},
        {SubgameType.GRILLING, "Grill"},
        {SubgameType.CHOPPING, "Chop"},
        {SubgameType.BOILING, "Boil"},
    };

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
