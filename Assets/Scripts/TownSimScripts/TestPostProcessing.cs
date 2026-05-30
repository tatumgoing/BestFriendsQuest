using MyBox;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class TestPostProcessing : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PostProcessingSetting setting;
    [SerializeField] private Volume volume;

    [ButtonMethod]
    public void QuickTest()
    {
        volume.profile = setting.VolumeProfile;            
    
        // Skybox
        RenderSettings.skybox = setting.Skybox;
        DynamicGI.UpdateEnvironment();   
    

        RenderSettings.ambientLight = setting.AmbientLightColor;
    }
}
