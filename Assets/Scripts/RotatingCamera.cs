using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; 
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
            cam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_MaxSpeed = topSpeed;
        }
        else
        {
            cam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_MaxSpeed = Mathf.Lerp(cam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_MaxSpeed, 0f, .25f);
        }

        if(cam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_MaxSpeed < 1)
        {
            cam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.m_MaxSpeed = 0;
        }
       
    }
}
