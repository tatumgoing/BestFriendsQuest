using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonGeneral : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public float initialX;
    public float initialY;

    public float travelDist;

    public void Start()
    {

        initialX = GetComponent<RectTransform>().anchoredPosition.x;
        initialY = GetComponent<RectTransform>().anchoredPosition.y;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(initialX, initialY - travelDist);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(initialX, initialY);

    }

} 
