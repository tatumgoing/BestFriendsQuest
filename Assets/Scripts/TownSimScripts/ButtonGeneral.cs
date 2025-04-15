using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonGeneral : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Sound clickSFX;

     float initialX;
     float initialY;

    public float travelDist;

    public void Start()
    {
        clickSFX = Instantiate(clickSFX);


        initialX = GetComponent<RectTransform>().anchoredPosition.x;
        initialY = GetComponent<RectTransform>().anchoredPosition.y;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        clickSFX.Play();
        GetComponent<RectTransform>().anchoredPosition = new Vector2(initialX, initialY - travelDist);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(initialX, initialY);

    }

} 
