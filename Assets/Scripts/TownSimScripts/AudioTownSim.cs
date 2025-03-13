using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTownManager : MonoBehaviour
{
   public AudioSource generalButtonSFX;

    public void PlayGeneralButtonSFX()
    {
        generalButtonSFX.Play();
    }
}
