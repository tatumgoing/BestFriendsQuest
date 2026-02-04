using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;
public class RotatingCamera : MonoBehaviour
{
    public float topSpeed;
    CinemachineVirtualCamera cam; 

    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject()) {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            } 

            cam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_MaxSpeed = topSpeed;
        }
        else
        {
            cam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_MaxSpeed = Mathf.Lerp(cam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_MaxSpeed, 0f, .25f);
        }

        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject()) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        if(cam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_MaxSpeed < 1)
        {
            cam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_MaxSpeed = 0;
        }
    }
}
