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
    public Sprite Icon;
    public float MaxScore;
    public int MoneyReward;

    public Difficulty Difficulty;

    public List<SubgameData> Subgames = new List<SubgameData>();

    // enum to verb pls
    static Dictionary<SubgameType, string> subgameToVerb =
    new Dictionary<SubgameType, string>() {
        {SubgameType.SITRRING, "Stir"},
        {SubgameType.GRILLING, "Grill"},
        {SubgameType.CHOPPING, "Chop"},
    };

    public string ReturnSteps()
    {
        int stepNumber = 1;
        string stepsStr = "";

        //for every subgame, add the number of step, the verb, and a line break
        foreach (SubgameData s in Subgames)
        { 
            stepsStr += stepNumber.ToString() + ". " + subgameToVerb[s.Type].ToString() + "\n";
            stepNumber++;
        }

        return stepsStr;
    }


}
