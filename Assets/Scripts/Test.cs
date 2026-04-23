using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using TMPro;
using UnityEngine;

public class Test : MonoBehaviour
{

    [ButtonMethod]
    public void SaveDict()
    {
        var newDict = new Dictionary<string, float>();
        for (int i = 0; i < 12; i++) {
            newDict.Add(i.ToString(), i + Random.Range(0, 1f));
        }

        SaveSystem.SaveHighscoreDictionary("TestDict", newDict);
    }

    [ButtonMethod]
    public void LoadDict()
    {
        var loadedDict = SaveSystem.LoadHighscoreDictionary("TestDict");
        print("Loaded dict:");
        foreach (var pair in loadedDict) {
            print(pair.Key + ": " + pair.Value);
        }
    }
}
