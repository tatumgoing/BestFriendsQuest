using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RollIcon : MonoBehaviour, IPointerClickHandler
{
    public bool isClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + " was clicked using EventSystem!");

        isClicked = true;
    }
}

