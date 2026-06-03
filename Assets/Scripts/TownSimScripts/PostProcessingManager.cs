using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessingManager : MonoBehaviour
{
    public static PostProcessingManager i;
    public PostProcessingSetting genericProcessingSetting;

    [SerializeField] private GameObject volumeManager;

    private Volume _volume;

    private void Awake()
    {
        i = this;
    }

    void Start()
    {
        if (!_volume) initialize();
    }

    private void initialize()
    {
        if (!_volume && volumeManager) _volume = volumeManager.GetComponent<Volume>();
    }

    public void SetGeneric(){

        if (!_volume) initialize();
        _volume.profile = genericProcessingSetting.VolumeProfile;

        if(RenderSettings.skybox != genericProcessingSetting.Skybox){
            // Skybox
            RenderSettings.skybox = genericProcessingSetting.Skybox;
            DynamicGI.UpdateEnvironment();
        }

        if(RenderSettings.ambientLight != genericProcessingSetting.AmbientLightColor)
        {
            RenderSettings.ambientLight = genericProcessingSetting.AmbientLightColor;
        }
    }

    public void SetPostProcessing(PostProcessingSetting newSettings){

        if (!_volume) initialize();
        if (_volume.profile != newSettings.VolumeProfile)
        {
            _volume.profile = newSettings.VolumeProfile;            
        }

        if(RenderSettings.skybox != newSettings.Skybox)
        {
            // Skybox
            RenderSettings.skybox = newSettings.Skybox;
            DynamicGI.UpdateEnvironment();   
        }

        if(RenderSettings.ambientLight != newSettings.AmbientLightColor)
        {
            RenderSettings.ambientLight = newSettings.AmbientLightColor;
        }
    }

    public void TestPostProcessing(PostProcessingSetting newSettings){

        _volume = volumeManager.GetComponent<Volume>();        
        _volume.profile = newSettings.VolumeProfile;            
    
            // Skybox
        RenderSettings.skybox = newSettings.Skybox;
        DynamicGI.UpdateEnvironment();   
    
        RenderSettings.ambientLight = newSettings.AmbientLightColor;

    }
}
