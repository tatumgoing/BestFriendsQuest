using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class RollMinigame : Subgame
{

    [SerializeField] private List<RectTransform> goodZones;
    [SerializeField] private List<RectTransform> greatZones;

    [SerializeField] private GameObject rollIcon;

    [SerializeField] private GameObject top;
    [SerializeField] private GameObject bottom;

    [SerializeField] private float currentSpeed;

    bool goingUp;

    override protected void Update()
    {
        base.Update();

        //put normal update code here.
        //add to _successTime to progress subgame.
        //max time for successtime (when the subgame marks itself as finished) is data.TargetTime

        MoveRoll();
        OutOfBounds();

        if (Input.GetMouseButtonDown(0))
        {
            ChangeDirection();
            CheckSuccess();
        }
    }

    public override void StartSubgame(SubgameData data)
    {
        base.StartSubgame(data);

        //ShowCam(0);

        //put code here that you want to run every time subgame is started

        currentSpeed = Data.RollSpeed;

        //audio
    }

    protected override void Initialize()
    {
        base.Initialize();

        //called just one, like 'start' but for subgame
        //for example, instnaitating your sound objects


    }

    private void OnDisable()
    {
       ResetCam();


    }

    private void MoveRoll()
    {
        if (goingUp)
        {
            Vector2 tempV = rollIcon.GetComponent<RectTransform>().anchoredPosition;

            tempV.y += currentSpeed * Time.deltaTime;

            rollIcon.GetComponent<RectTransform>().anchoredPosition = tempV;
        }
        else {

            Vector2 tempV = rollIcon.GetComponent<RectTransform>().anchoredPosition;

            tempV.y -= currentSpeed * Time.deltaTime;

            rollIcon.GetComponent<RectTransform>().anchoredPosition = tempV;

        }
        
    }

    private void OutOfBounds()
    {
        if (rollIcon.GetComponent<RectTransform>().anchoredPosition.y >= top.GetComponent<RectTransform>().anchoredPosition.y)
        {
            ResetRoll();
           
        }
        else if(rollIcon.GetComponent<RectTransform>().anchoredPosition.y <= bottom.GetComponent<RectTransform>().anchoredPosition.y)
        {
            ResetRoll();
           
        }


    }
    private void ResetRoll()
    {
        rollIcon.GetComponent<RectTransform>().anchoredPosition= new Vector2 (0, 0);
        
        currentSpeed = Data.RollSpeed;
        SubtractScore();

    }

    private void ChangeDirection()
    {
        goingUp = !goingUp;
        Debug.Log("Changing Direction");
    }

    private void CheckSuccess()
    {

        if (CheckGreat())
        {
            AddScore(true);
        }
        else if (CheckGood())
        {
            AddScore(false);
        }
        else
        {
            Debug.Log("Miss");

            SubtractScore();
        }

    }

    private bool CheckGreat()
    {
        foreach (RectTransform zone in greatZones)
        {
            float upperBound = zone.GetComponent<RectTransform>().anchoredPosition.y + (zone.rect.height / 2);
            float lowerBound = zone.GetComponent<RectTransform>().anchoredPosition.y - (zone.rect.height / 2);

            float rollLocation = rollIcon.GetComponent<RectTransform>().anchoredPosition.y;

            Debug.Log(upperBound + ", " + rollLocation + ", " + lowerBound);

            if (upperBound >= rollLocation && rollLocation >= lowerBound)
            {
                Debug.Log("Great");

                return true;
            }
        }

        return false;
    }

    private bool CheckGood()
    {
        foreach (RectTransform zone in goodZones)
        {
            float upperBound = zone.GetComponent<RectTransform>().anchoredPosition.y + (zone.rect.height / 2);
            float lowerBound = zone.GetComponent<RectTransform>().anchoredPosition.y - (zone.rect.height / 2);

            float rollLocation = rollIcon.GetComponent<RectTransform>().anchoredPosition.y;


            if (upperBound >= rollLocation && rollLocation >= lowerBound)
            {
                Debug.Log("Good");
             
                return true;
            }
        }

        return false;
    }
    private void AddScore(bool isGreat)
    {
        currentSpeed += Data.SpeedUp;

        if (isGreat)
        {
            SuccessTime += Data.RollGreat;
        }
        else
        {
            SuccessTime += Data.RollGood;
        }
    }

    private void SubtractScore()

    {        
        currentSpeed += Data.RollSpeed;

        SuccessTime -= Data.RollPenalty;
    }




}
