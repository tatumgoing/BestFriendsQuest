using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
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


    void Start()
    {
        iconPosX = GetComponent<RectTransform>().anchoredPosition.x;
        iconPosY = GetComponent<RectTransform>().anchoredPosition.y;
        defRot= GetComponent<RectTransform>().rotation.z;
    }

    void Update()
    {
        var newIconPosY = iconPosY;
        var newIconPosX = iconPosX;
        var newIconRot = defRot;

        if (doesRot)
        {
            newIconRot = (maxRot - minRot)/2 * Mathf.Sin(rotSpeed * (Time.time)) + defRot + ((maxRot + minRot) / 2);
            GetComponent<RectTransform>().rotation= Quaternion.Euler(0, 0, newIconRot);
        }
        if (doesVert) 
        {
             newIconPosY = (maxVert - minVert)/2 * Mathf.Sin(vertSpeed * (Time.time)) + iconPosY + ((maxVert + minVert) / 2);
        }
        if (doesHoriz)
        {
            newIconPosX = (maxHoriz-minHoriz)/2 * Mathf.Sin(horizSpeed * (Time.time)) + iconPosX + ((maxHoriz + minHoriz) / 2);
        }
        GetComponent<RectTransform>().anchoredPosition = new Vector2(newIconPosX, newIconPosY);
    }
}
