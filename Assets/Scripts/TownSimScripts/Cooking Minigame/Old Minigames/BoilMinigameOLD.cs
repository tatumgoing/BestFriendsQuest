using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoilMinigameOLD : MonoBehaviour
{
    public MinigameManager manager;

    public GameObject cookingBar;

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

    [Header("Audio")]

    [SerializeField]private Sound boilNormal;
    [SerializeField]private Sound boilLoud;

    void Start()
    {
        boilNormal = Instantiate(boilNormal);
        boilLoud = Instantiate(boilLoud);

        boilNormal.Play();
        boilLoud.Play();

        //BAD BAD BAD BAD BAD KILL
        manager = FindFirstObjectByType<MinigameManager>();


        iconPosX = barIcon.GetComponent<RectTransform>().localPosition.x;
        iconPosY = barIcon.GetComponent<RectTransform>().localPosition.y;

        foreach (TargetZone target in cookingBar.GetComponentsInChildren<TargetZone>()) { 
            targets.Add(target);
            Debug.Log(target.GetComponent<RectTransform>().localPosition);
            //target.SetBounds(target.GetComponent<RectTransform>().localPosition.x);
        }

        upperBound = cookingBar.GetComponent<RectTransform>().anchoredPosition.x + cookingBar.GetComponent<RectTransform>().sizeDelta.x / 2;
        lowerBound = cookingBar.GetComponent<RectTransform>().anchoredPosition.x - cookingBar.GetComponent<RectTransform>().sizeDelta.x / 2;


        tempIcon.GetComponent<Image>().sprite = manager.characterSelectionMenu.selectedCharacter.Icon;

    }

    void Update()
    {
        /*if (manager.currentTimer != null && manager.currentTimer.timerActive)
        {
            CheckSpeed();
        }

        if (manager.currentTimer != null && manager.currentTimer.timerActive)
        {   
            if (CheckTargets())
            {
                manager.currentTimer.AddProgress(addScore * Time.deltaTime);

                boilLoud.SetPercentVolume(100, 10 * Time.deltaTime);
                boilNormal.SetPercentVolume(0, 10 * Time.deltaTime);
            }
            else
            {
                manager.currentTimer.RemoveProgress(penaltyScore * Time.deltaTime);

                boilLoud.SetPercentVolume(0, 10 * Time.deltaTime);
                boilNormal.SetPercentVolume(100, 10 * Time.deltaTime);
            }
        }

        if (!manager.currentTimer.timerActive)
        {
            boilLoud.SetPercentVolume(0, 10 * Time.deltaTime);
            boilNormal.SetPercentVolume(0, 10 * Time.deltaTime);
        }*/

    }

    public void CheckSpeed()
    {
        if (Input.GetKey("space") || Input.GetMouseButton(0))
        {
            iconVelocity += accSpeed * Time.deltaTime;
        }
        else if (barIcon.GetComponent<RectTransform>().localPosition.x ==  lowerBound)
        {
            iconVelocity = 0;
        }
        else
        {
            iconVelocity -= decSpeed * Time.deltaTime;
        }

        iconVelocity = Mathf.Clamp(iconVelocity, minSpeed, maxSpeed);

        iconPosX += iconVelocity * Time.deltaTime;

        iconPosX = Mathf.Clamp(iconPosX, lowerBound, upperBound);
        barIcon.GetComponent<RectTransform>().localPosition = new Vector2(iconPosX, iconPosY);

        //Debug.Log("Checking Speed: " +  Time.deltaTime + " " + iconVelocity);
    }
    public bool CheckTargets()
    {
        bool inRange = false;
        foreach (TargetZone target in targets)
        {
            
                if (barIcon.GetComponent<RectTransform>().localPosition.x >= target.lowerBound && barIcon.GetComponent<RectTransform>().localPosition.x <= target.upperBound)
                {
                    inRange = true;

                    
                }
            
        }

        //Debug.Log(inRange);

        return inRange;
    }

    



}
