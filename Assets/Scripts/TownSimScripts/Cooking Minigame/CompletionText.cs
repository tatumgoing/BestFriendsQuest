using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletionText : MonoBehaviour
{
    [SerializeField] Sound completionSFX;
    private void OnEnable()
    {
       completionSFX = Instantiate(completionSFX);

    }

    public void PlayCompletionSFX()
    {
        completionSFX.Play();

    }
}
