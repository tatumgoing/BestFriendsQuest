using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingHighscores : MonoBehaviour
{
    [SerializeField] private List<RecipeData> _recipes;    //load from resources when thats a thing


    void Start()
    {
        UpdateCookingHighscores();
    }

    public void UpdateCookingHighscores()
    {
        Dictionary<string, float> tempDict = SaveSystem.LoadHighscoreDictionary("Cooking");

        foreach (RecipeData r in _recipes) {

            if (!tempDict.ContainsKey(r.Name))
            {
                tempDict.Add(r.Name, 0);
            }

        }

        Debug.Log(string.Join(", ", tempDict)); 
        
        SaveSystem.SaveHighscoreDictionary("Cooking", tempDict);
    }

    
    //public void NewCookingHighscore(string recipe, float newScore)
    //{
    //    Dictionary<string, float> tempDict = SaveSystem.LoadHighscoreDictionary("Cooking");

    //    if(tempDict[recipe] <= newScore)
    //    {
    //        tempDict[recipe] = newScore;
    //        SaveSystem.SaveHighscoreDictionary("Cooking", tempDict);
    //    }

    //}
}
