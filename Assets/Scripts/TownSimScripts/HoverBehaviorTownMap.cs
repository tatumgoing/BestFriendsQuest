using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverBehaviorTownMap : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    HoverTextTownMap i;

    [SerializeField] string locationName;
    [SerializeField] string locationDescription;

    public Sprite hoverImage;
    public Sprite defaultImage;

    float width;
    float height;

    void Start()
    {
        GetComponent<Image>().sprite = defaultImage;

        i = HoverTextTownMap.i;

        //StartCoroutine(SetBounds());

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (i != null)
        {
            i.ActivateMenu(locationName, locationDescription);
        }
        GetComponent<Image>().sprite = hoverImage;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (i != null)
        {
            i.StartCoroutine("DeactivateMenu");
        }

        GetComponent<Image>().sprite = defaultImage;
    }


}
