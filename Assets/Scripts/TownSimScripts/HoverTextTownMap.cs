using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class HoverTextTownMap : MonoBehaviour
{

    public TMP_Text locationName;
    public TMP_Text locationDescription;

    public static HoverTextTownMap i;
    void Awake()
    {
        HoverTextTownMap.i = this;
    }
    public  void UpdateName(string newName)
    {
        locationDescription.text = newName;
    }

    public void UpdateDescription(string newDesc) 
    { 
    
        locationDescription.text = newDesc;
    }
}
