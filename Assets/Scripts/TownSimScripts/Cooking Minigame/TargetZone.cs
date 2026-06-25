using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetZone : MonoBehaviour
{
    public float upperBound;
    public float lowerBound;

    public float width;
    //public float height;

    // Start is called before the first frame update
    void Awake()
    {
         //width = GetComponent<RectTransform>().sizeDelta.x;
        //height = GetComponent<RectTransform>().sizeDelta.y;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBounds(float position, float length, float parentLength)
    {
        float center = 2*(position/parentLength);

        //Debug.Log("Setting Bounds");
        
        upperBound = center + (length);
        lowerBound = center - (length);

        //Debug.Log("lower: " + lowerBound + " upper: " + upperBound);
    }

    public void ChangeTargetWidth(float newWidth)
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x * newWidth, GetComponent<RectTransform>().sizeDelta.y);
    }

    public void ChangeTargetHeight(float newHeight)
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, GetComponent<RectTransform>().sizeDelta.y * newHeight);
    }

    public void MoveTarget(float newX, float newY)
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(newX, newY);
    }
}
