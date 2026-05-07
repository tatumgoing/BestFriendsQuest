using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ParkPostProcessing : MonoBehaviour
{


    [SerializeField] private PostProcessingSetting daySetting;
    [SerializeField] private PostProcessingSetting eveningSetting;
    [SerializeField] private PostProcessingSetting nightSetting;
    private PostProcessingManager postProcessingManager;



    DateTime currentTime;


    [SerializeField] private GameObject volumeManager;
    Volume volume;

    void OnEnable()
    {
        postProcessingManager = PostProcessingManager.i;

        volume = volumeManager.GetComponent<Volume>();

        currentTime = System.DateTime.Now;
        Debug.Log(currentTime.Hour);

        if (currentTime.Hour >= 5 && currentTime.Hour <= 17)
        {
            postProcessingManager.SetPostProcessing(daySetting);
           
        }
        else if (currentTime.Hour >= 4 && currentTime.Hour <= 19)
        {
            postProcessingManager.SetPostProcessing(eveningSetting);   

        }
        else {
            postProcessingManager.SetPostProcessing(nightSetting);
            
        }

    }

    void OnDisable()
    {
        postProcessingManager.SetGeneric();
    }

    
}
