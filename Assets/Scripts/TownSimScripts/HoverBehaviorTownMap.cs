using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverBehaviorTownMap : MonoBehaviour
{
    [SerializeField] HoverTextTownMap i;

    [SerializeField] string locationName;
    [SerializeField] string locationDescription;

    public float width;
    public float height;

    void Awake()
    {
         width = GetComponent<RectTransform>().sizeDelta.x;
         height = GetComponent<RectTransform>().sizeDelta.y;
    }
    void Start()
    {
        i = HoverTextTownMap.i;
        //SetBounds(GetComponent<RectTransform>().position.x, GetComponent<RectTransform>().position.y);
    }
    void Update()
    {
        if (width==height)
        {
            
        }
        //check if mouse is over, then pass to HoverTextTownMap
    }


}
