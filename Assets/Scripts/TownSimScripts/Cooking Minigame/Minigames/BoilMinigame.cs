using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoilMinigame : MonoBehaviour
{
    public MinigameManager manager;

    public GameObject cookingBar;

    public bool isHorizontal;

    public List<TargetZone> targets = new List<TargetZone>();

    public GameObject tempIcon;

    [Header("Scoring")]

    public float addScore;
    public float penaltyScore;

    [Header("Icon")]
    public GameObject barIcon;
    public float iconPosX;
    public float iconPosY;

    [Header("Animation")]

    public float maxSpeed;
    public float minSpeed;

    public float accSpeed;
    public float decSpeed;
    public float iconVelocity= 0;

    float upperBound;
    float lowerBound;




    // Start is called before the first frame update
    void Start()
    {
        //BAD BAD BAD BAD BAD KILL
        manager= FindFirstObjectByType<MinigameManager>();


        iconPosX = barIcon.GetComponent<RectTransform>().localPosition.x;
        iconPosY = barIcon.GetComponent<RectTransform>().localPosition.y;

        foreach (TargetZone target in cookingBar.GetComponentsInChildren<TargetZone>()) { 
            targets.Add(target);
            target.SetBounds(target.GetComponent<RectTransform>().localPosition.x, isHorizontal);
        }

        upperBound = cookingBar.GetComponent<RectTransform>().anchoredPosition.x + cookingBar.GetComponent<RectTransform>().sizeDelta.x / 2;
        lowerBound = cookingBar.GetComponent<RectTransform>().anchoredPosition.x - cookingBar.GetComponent<RectTransform>().sizeDelta.x / 2;

    }

    // Update is called once per frame
    void Update()
    {

        CheckSpeed();

        if (CheckTargets())
        {
            manager.currentTimer.AddProgress(addScore);
        }
        else
        {
            manager.currentTimer.RemoveProgress(penaltyScore);
        }
    }

    public void CheckSpeed()
    {
        if (Input.GetKey("space"))
        {
            iconVelocity += accSpeed;
        }
        else
        {
            iconVelocity -= decSpeed;
        }

        iconVelocity = Mathf.Clamp(iconVelocity, minSpeed, maxSpeed);

        iconPosX += iconVelocity * Time.deltaTime;

        iconPosX = Mathf.Clamp(iconPosX, lowerBound, upperBound);
        barIcon.GetComponent<RectTransform>().localPosition = new Vector2(iconPosX, iconPosY);
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

        //Debug.Log(inRange);

        return inRange;
    }


}
