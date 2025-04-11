using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopTabs : MonoBehaviour
{
    public Sprite defaultState;
    public Sprite clickedState;

    public bool selected = false; 

    void Update()
    {
        if (!selected)
        {
            GetComponent<Image>().sprite = defaultState;
        }
        else
        {
            GetComponent<Image>().sprite = clickedState;

        }
    }

    public void TabIsClicked()
    {
        selected = true;
    }
  
}
