using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessingManager : MonoBehaviour
{

    public static PostProcessingManager i;

    public PostProcessingSetting genericProcessingSetting;

    [SerializeField] private GameObject volumeManager;
    Volume volume;



    // Start is called before the first frame update
    void Start()
    {
        i = this;

       volume = volumeManager.GetComponent<Volume>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGeneric(){

        volume.profile = genericProcessingSetting.VolumeProfile;

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

        if(volume.profile != newSettings.VolumeProfile)
        {
            volume.profile = newSettings.VolumeProfile;            
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

        volume = volumeManager.GetComponent<Volume>();

        
        volume.profile = newSettings.VolumeProfile;            
    
            // Skybox
        RenderSettings.skybox = newSettings.Skybox;
        DynamicGI.UpdateEnvironment();   
    

        RenderSettings.ambientLight = newSettings.AmbientLightColor;

        }
}
