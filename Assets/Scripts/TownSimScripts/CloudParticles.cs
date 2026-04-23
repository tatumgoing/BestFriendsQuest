using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class CloudParticles : MonoBehaviour
{
    private float endTime;
    [SerializeField] private float duration;
    [SerializeField] private ParticleSystem particles;

    // Update is called once per frame

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }
    void Update()
    {
        if (Time.time > endTime) {
            //Debug.Log("False");
            //this.gameObject.SetActive(false);
            particles.Stop();

        }
        else
        {
            //Debug.Log("True");
            //this.gameObject.SetActive(true);
            particles.Play();

        }
    }
    

    public void SpawnCharacterSelectCloud()
    {
        endTime = Time.time + duration;
        //Debug.Log(endTime + " " + Time.time);
        this.gameObject.SetActive(true);

    }
}
