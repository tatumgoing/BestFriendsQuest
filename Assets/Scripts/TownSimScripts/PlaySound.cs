using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] Sound sound;

    public void OnEnable()
    {
        sound = Instantiate(sound);
    }
    public void SimplePlaySound()
    {
        sound.Play();
    }
    public void SimpleStopSound()
    {
        sound.Stop();
    }
}
