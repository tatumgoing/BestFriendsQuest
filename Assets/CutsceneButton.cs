using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneButton : MonoBehaviour
{
    public void StartCutscene()
    {
        CutsceneManager.i.StartCutscene();

        gameObject.SetActive(false);
    }
}
