using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoilMinigame : Subgame
{
    private float iconPosX, iconPosY;
    [SerializeField] private Slider boilSlider;
    [SerializeField] private GameObject sliderIcon;
    [SerializeField] private TargetZone target;

    [Header("Animation")]

    public float iconVelocity = 0;

    [Header("Audio")]

    [SerializeField] private Sound boilNormal;
    [SerializeField] private Sound boilLoud;

    override protected void Update()
    {
        base.Update();

        //put normal update code here.
        //add to _successTime to progress subgame.
        //max time for su 1`ccesstime (when the subgame marks itself as finished) is data.TargetTime

        CheckSpeed();

        if (CheckTargets())
        {
            SuccessTime += Time.deltaTime;
            boilLoud.SetPercentVolume(100, 10 * Time.deltaTime);
            boilNormal.SetPercentVolume(0, 10 * Time.deltaTime);
        }
        else
        {
            boilLoud.SetPercentVolume(0, 10 * Time.deltaTime);
            boilNormal.SetPercentVolume(100, 10 * Time.deltaTime);
        }
        

    }

    public override void StartSubgame(SubgameData data)
    {
        base.StartSubgame(data);

        iconPosX = sliderIcon.GetComponent<RectTransform>().anchoredPosition.x;
        iconPosY = sliderIcon.GetComponent<RectTransform>().anchoredPosition.y;

        //put code here that you want to run every time subgame is started

        target.MoveTarget(Data.BoilTargetPosition, 0.0f);
        target.ChangeTargetWidth(Data.BoilTargetScale);

        float position = target.GetComponent<RectTransform>().anchoredPosition.x;
        float length = target.GetComponent<RectTransform>().sizeDelta.x / boilSlider.GetComponent<RectTransform>().sizeDelta.x;
        float parentLength = boilSlider.GetComponent<RectTransform>().sizeDelta.x;

        target.SetBounds(position, length, parentLength);

        //audio

        boilNormal.PlaySilent();
        boilLoud.PlaySilent();

    }

    protected override void Initialize()
    {
        base.Initialize();

        //called just one, like 'start' but for subgame
        //for example, instnaitating your sound objects

        boilNormal = Instantiate(boilNormal);
        boilLoud = Instantiate(boilLoud);

    }

    private void OnDisable()
    {
        boilNormal.Stop();
        boilLoud.Stop();
    }

    public void CheckSpeed()
    {
        if (Input.GetKey("space") || Input.GetMouseButton(0))
        {
            iconVelocity += Data.BoilAccSpeed * Time.deltaTime;
        }
        else if (boilSlider.value == boilSlider.minValue)
        {
            iconVelocity = 0;
        }
        else
        {
            iconVelocity -= Data.BoilDeccSpeed * Time.deltaTime;
        }

        iconVelocity = Mathf.Clamp(iconVelocity, Data.BoilMinSpeed, Data.BoilMaxSpeed);

        boilSlider.value += iconVelocity * Time.deltaTime;

    }
    private bool CheckTargets()
    {
        if (boilSlider.value >= target.lowerBound && boilSlider.value <= target.upperBound)
        {
            return true;
        }

        return false;
    }
}
