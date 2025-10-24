using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class VignetteButton : MonoBehaviour
{
    public VignetteManager manager;
    public Vignette vignetteImport;
    //public GameObject cameraTarget;
    public void Start()
    {
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        manager.StartVignette(vignetteImport);

    }
    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }

}
