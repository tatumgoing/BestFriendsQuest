using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationButton : MonoBehaviour
{
    public GameObject newSceneUI;

    public TownGameManager gameManager;

    public void Start()
    {
        //Debug.Log(this + ": " + newScene + newSceneUI);

    }
    public void ClickNavigation()
    {
        if (newSceneUI != null)
        {
            gameManager.ChangeScene(newSceneUI);
        }

        //Debug.Log("Going to: " + newScene + newSceneUI);
    }

    public void LoadCharacterCreator()
    {
        SceneManager.LoadScene(1);
    }
        
}
