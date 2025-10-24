using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; 

public class VignetteCameras : MonoBehaviour
{
    public Dictionary<string, List<CinemachineVirtualCamera>> vignetteCameras = new Dictionary<string, List<CinemachineVirtualCamera>>();

    [SerializeField] public List<CinemachineVirtualCamera> parkCameras = new List<CinemachineVirtualCamera>();

    void Awake()
    {
        vignetteCameras.Add("Park", parkCameras);
    }

}
