using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class VignetteButton : MonoBehaviour, IPointerClickHandler
{
    public VignetteManager manager;
    public Vignette vignetteImport;
    //public GameObject cameraTarget;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicky!");
        if (!manager.isPlaying)
        {
            manager.StartVignette(vignetteImport);
        }

    }
    void Update()
    {
        transform.LookAt(Camera.main.transform);

        GetComponent<SpriteRenderer>().enabled= !manager.isPlaying;
    }

}
