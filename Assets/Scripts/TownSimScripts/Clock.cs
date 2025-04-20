using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Clock : MonoBehaviour
{

    public TMP_Text timeDisplay;
    void Update()
    {
        timeDisplay.text = GetTime() + " " + GetAM();
    }

    private string GetTime()
    {
        return DateTime.Now.ToString("h:mm");
       
    }

    private string GetAM()
    {
        if (DateTime.Now.Hour > 12)
        {
            return("PM");
        }
        else
        {
            return("AM");
        }
    }
}

