using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "PostProcessingSetting", menuName = "Post Processing Setting", order = 3)]
public class PostProcessingSetting : ScriptableObject
{
    [SerializeField] public VolumeProfile VolumeProfile;
    [SerializeField] public UnityEngine.Color AmbientLightColor = new UnityEngine.Color(0.84f, 0.84f, 0.84f, 1.0f);
    [SerializeField] public  Material Skybox;

}
