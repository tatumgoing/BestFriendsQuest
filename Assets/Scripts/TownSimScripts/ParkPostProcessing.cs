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
    [SerializeField] private PostProcessingSetting genericSetting;


    DateTime currentTime;


    [SerializeField] private GameObject volumeManager;
    Volume volume;
    void OnEnable()
    {

        volume = volumeManager.GetComponent<Volume>();

        currentTime = System.DateTime.Now;
        Debug.Log(currentTime.Hour);

        if (currentTime.Hour >= 5 && currentTime.Hour <= 17)
        {
            SetDay();
           
        }
        else if (currentTime.Hour >= 4 && currentTime.Hour <= 19)
        {
            SetEvening();

        }
        else {

            SetNight();
        }

    }

    void OnDisable()
    {
        SetGeneric();
    }

    private void SetDay()
    {
        Debug.Log("Setting Day");

        volume.profile = daySetting.VolumeProfile;

        // Skybox
        RenderSettings.skybox = daySetting.Skybox;
        DynamicGI.UpdateEnvironment();

    }

    private void SetEvening()
    {
        Debug.Log("Setting Day");

        volume.profile = eveningSetting.VolumeProfile;

        // Skybox
        RenderSettings.skybox = eveningSetting.Skybox;
        DynamicGI.UpdateEnvironment();

    }

    private void SetNight()
    {
        Debug.Log("Setting Night");


        volume.profile = nightSetting.VolumeProfile;

        // Skybox
        RenderSettings.skybox = nightSetting.Skybox;
        DynamicGI.UpdateEnvironment();
    }

    private void SetGeneric()
    {
        volume.profile = genericSetting.VolumeProfile;

        // Skybox
        RenderSettings.skybox = genericSetting.Skybox;
        DynamicGI.UpdateEnvironment();
    }
}
