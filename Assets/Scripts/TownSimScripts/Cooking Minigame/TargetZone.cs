using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetZone : MonoBehaviour
{
    public float upperBound;
    public float lowerBound;

    public float width;
    public float height;

    // Start is called before the first frame update
    void Awake()
    {
         width = GetComponent<RectTransform>().sizeDelta.x;
         height = GetComponent<RectTransform>().sizeDelta.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBounds(float center, bool isHorizontal)
    {

        Debug.Log("Setting Bounds");
        if (isHorizontal)
        {
            upperBound = center + (width / 2);
            lowerBound = center - (width / 2);

        }
        else
        {
            upperBound = center + (height / 2);
            lowerBound = center - (height / 2);
        }
    }
}
