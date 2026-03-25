using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudParticles : MonoBehaviour
{
    private float endTime;
    [SerializeField] private float duration;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > endTime) {
            Debug.Log("False");
            this.gameObject.SetActive(false);

        }
        else
        {
            Debug.Log("True");
            this.gameObject.SetActive(true);
        }
    }
    

    public void SpawnCharacterSelectCloud()
    {
        endTime = Time.time + duration;
        Debug.Log(endTime + " " + Time.time);
        this.gameObject.SetActive(true);

    }
}
