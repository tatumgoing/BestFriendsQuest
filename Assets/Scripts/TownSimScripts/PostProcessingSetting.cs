using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "PostProcessingSetting", menuName = "Post Processing Setting", order = 3)]
public class PostProcessingSetting : ScriptableObject
{
    [SerializeField] public VolumeProfile VolumeProfile;
    [SerializeField] public  Material Skybox;


}
