using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class VignetteButton : MonoBehaviour
{
    public VignetteManager manager;
    public Vignette vignetteImport;
    public List<CinemachineVirtualCamera> cameraImport;

    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => manager.StartVignette(vignetteImport, cameraImport));
    }
}
