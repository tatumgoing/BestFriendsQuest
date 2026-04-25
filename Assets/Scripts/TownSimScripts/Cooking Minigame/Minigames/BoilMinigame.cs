using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoilMinigame : Subgame
{
    [SerializeField] private Slider boilSlider;
    [SerializeField] private GameObject sliderIcon;
    [SerializeField] private TargetZone target;

    override protected void Update()
    {
        base.Update();

        //put normal update code here.
        //add to _successTime to progress subgame.
        //max time for successtime (when the subgame marks itself as finished) is data.TargetTime

    }

    public override void StartSubgame(SubgameData data)
    {
        base.StartSubgame(data);

        //put code here that you want to run every time subgame is started

    }

    protected override void Initialize()
    {
        base.Initialize();

        //called just one, like 'start' but for subgame
        //for example, instnaitating your sound objects
    }

    private bool CheckTargets()
    {
        if (sliderIcon.GetComponent<RectTransform>().localPosition.x >= target.lowerBound && sliderIcon.GetComponent<RectTransform>().localPosition.x <= target.upperBound)
        {
            return true;
        }

        return false;
    }
}
