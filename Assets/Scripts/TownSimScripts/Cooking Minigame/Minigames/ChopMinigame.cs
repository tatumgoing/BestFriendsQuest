using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopMinigame : MonoBehaviour
{
    public MinigameManager manager;

    public GameObject cookingBar;

    public bool isHorizontal;

    public List<TargetZone> targets = new List<TargetZone>();

    [Header("Scoring")]

    public float addScore;
    public float penaltyScore;

    [Header("Icon")]
    public GameObject barIcon;
    public float iconPosX;
    public float iconPosY;

    [Header("Animation")]
    public float barSpeed; // or period
    public float amplitude; // one half width of bar
    public float shift; //should be the middle of the bar




    // Start is called before the first frame update
    void Start()
    {
        iconPosX = barIcon.GetComponent<RectTransform>().anchoredPosition.x;
        iconPosY = barIcon.GetComponent<RectTransform>().anchoredPosition.y;


        if (isHorizontal)
        {
            amplitude = cookingBar.GetComponent<RectTransform>().sizeDelta.x /2 ;
            shift = cookingBar.GetComponent<RectTransform>().anchoredPosition.x;

        }

        foreach (TargetZone target in cookingBar.GetComponentsInChildren<TargetZone>()) { 
            targets.Add(target);
            target.SetBounds(target.GetComponent<RectTransform>().anchoredPosition.x, isHorizontal);
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (isHorizontal) {
            iconPosX = (amplitude * Mathf.Sin(barSpeed * (Time.time)) + shift);
            barIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(iconPosX, iconPosY);
        }

        if (Input.GetKeyDown("space"))
        {
            if (CheckTargets())
            {
                manager.currentTimer.AddProgress(addScore); 
            }
            else
            {
                manager.currentTimer.RemoveProgress(penaltyScore);
            }

            Debug.Log(CheckTargets());
        }
    }


    public bool CheckTargets()
    {
        bool inRange = false;
        foreach (TargetZone target in targets)
        {
            if (isHorizontal)
            {
                if (iconPosX >= target.lowerBound && iconPosX <= target.upperBound)
                {
                    inRange = true;
                }
            }
        }

        Debug.Log(inRange);

        return inRange;
    }


}
