using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ChopMinigame : Subgame
{
    private float iconPosX, iconPosY;
    [SerializeField] private Slider chopSlider;
    [SerializeField] private GameObject sliderIcon;
    [SerializeField] private TargetZone target;

    [Header("Sounds")]
    [SerializeField] private Sound chopSFX;
    [SerializeField] private Sound wrongSFX;


    override protected void Update()
    {
        base.Update();

        //put noremal update code here.
        //add to _successTime to progress subgame.
        //max time for successtime (when the subgame marks itself as finished) is data.TargetTime

        if (Data != null) chopSlider.value = Mathf.Sin(Data.ChopBarSpeed * (Time.time)); 

        if (Input.GetKeyDown("space") || Input.GetMouseButtonDown(0))
        {
            Chop();
        }
    }

    public override void StartSubgame(SubgameData data)
    {

        base.StartSubgame(data);

        iconPosX = sliderIcon.GetComponent<RectTransform>().anchoredPosition.x;
        iconPosY = sliderIcon.GetComponent<RectTransform>().anchoredPosition.y;

        //put code here that you want to run every time subgame is started

        target.MoveTarget(Data.ChopTargetPosition, 0.0f);
        target.ChangeTargetWidth(Data.ChopTargetScale);

        float position = target.GetComponent<RectTransform>().anchoredPosition.x;
        float length = target.GetComponent<RectTransform>().sizeDelta.x / chopSlider.GetComponent<RectTransform>().sizeDelta.x;
        float parentLength = chopSlider.GetComponent<RectTransform>().sizeDelta.x;

        target.SetBounds(position, length, parentLength);
    }

    protected override void Initialize()
    {
        base.Initialize();

        chopSFX = Instantiate(chopSFX);
        wrongSFX = Instantiate(wrongSFX);

        //some bullshit about targetzone ratio to slider 

        //called just one, like 'start' but for subgame
        //for example, instnaitating your sound objects
    }

    public void Chop()
    {

        if (CheckTargets())
        {
            SuccessTime += Data.ChopValue;
            chopSFX.Play(oneShot: true);
        }
        else
        {
            SuccessTime -= Data.ChopPenalty;
            wrongSFX.Play(oneShot: true);
        }

    }

    public bool CheckTargets()
    {
        if (chopSlider.value >= target.lowerBound && chopSlider.value <= target.upperBound)
        {
            return true;
        }

        return false;
    }

}
