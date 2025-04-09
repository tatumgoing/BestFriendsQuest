using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class FunAnimator : MonoBehaviour
{
    [Header("Rotation")]

    public bool doesRot;
    public float minRot;
    public float maxRot;
    public float rotSpeed;

    private float defRot;

    [Header("Vertical")]
    public bool doesVert;
    public float minVert;
    public float maxVert;
    public float vertSpeed;

    private float iconPosY;



    [Header("Horizontal")]
    public bool doesHoriz;
    public float minHoriz;
    public float maxHoriz;
    public float horizSpeed;

    private float iconPosX;

    [Header("Scale")]
    public bool doesScale;
    public float minScale;
    public float maxScale;
    public float scaleSpeed;

    private Vector3 defScale;


    void Start()
    {
        iconPosX = GetComponent<RectTransform>().anchoredPosition.x;
        iconPosY = GetComponent<RectTransform>().anchoredPosition.y;
        defRot= GetComponent<RectTransform>().rotation.z;
        defScale = GetComponent<RectTransform>().localScale;
    }

    void Update()
    {
        var newIconPosY = iconPosY;
        var newIconPosX = iconPosX;
        var newIconRot = defRot;

        if (doesRot)
        {
            GetComponent<RectTransform>().rotation= Quaternion.Euler(0, 0, SinAnimator(maxRot, minRot, rotSpeed, defRot));
        }
        if (doesVert)
        {
             newIconPosY = SinAnimator( maxVert, minVert, vertSpeed, iconPosY);
        }
        if (doesHoriz)
        {
            newIconPosX = SinAnimator(maxHoriz, minHoriz, horizSpeed, iconPosX); 
        }
        GetComponent<RectTransform>().anchoredPosition = new Vector2(newIconPosX, newIconPosY);
        if (doesScale)
        {
            GetComponent<RectTransform>().localScale = new Vector3 (SinAnimator(maxScale, minScale, scaleSpeed, defScale.x-1), SinAnimator(maxScale, minScale, scaleSpeed, defScale.y-1), 1);
        }
    }

    float SinAnimator(float max, float min, float speed, float start)
    {
        var newVal = (max - min) / 2 * Mathf.Sin(speed * (Time.time)) + start + ((max + min) / 2);

        return newVal;
    }

   
}
