using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverBehaviorTownMap : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    HoverTextTownMap i;

    [SerializeField] string locationName;
    [SerializeField] string locationDescription;

    float width;
    float height;

    void Start()
    {
        i = HoverTextTownMap.i;

        //StartCoroutine(SetBounds());

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        i.ActivateMenu(locationName, locationDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        i.StartCoroutine("DeactivateMenu");
    }


}
