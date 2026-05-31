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
    [SerializeField] private GameObject volumeManager;

    private PostProcessingManager _postProcessingManager;
    private DateTime _currentTime;
    private bool _initialized;
    private Volume _volume;

    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        Initialize();
    }

    void Initialize()
    {
        if (_initialized) return;

        _postProcessingManager = PostProcessingManager.i;
        if (_postProcessingManager == null) return;

        _initialized = true;

        _volume = volumeManager.GetComponent<Volume>();
        _currentTime = System.DateTime.Now;

        if (_currentTime.Hour >= 5 && _currentTime.Hour <= 17)
        {
            _postProcessingManager.SetPostProcessing(daySetting);
           
        }
        else if (_currentTime.Hour >= 4 && _currentTime.Hour <= 19)
        {
            _postProcessingManager.SetPostProcessing(eveningSetting);   

        }
        else {
            _postProcessingManager.SetPostProcessing(nightSetting);
            
        }
    }

    void OnDisable()
    {
        _postProcessingManager?.SetGeneric();
    }    
}
