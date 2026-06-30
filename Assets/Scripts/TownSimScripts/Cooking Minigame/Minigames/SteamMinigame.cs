using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SteamMinigame : Subgame
{
    float centerX;
    float centerY;

    List<SteamCloud> steamclouds = new List<SteamCloud>();

    [SerializeField] SteamCloud cloudPrefab;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] float padding= 50.0f;


    override protected void Update()
    {
        base.Update();

        //put normal update code here.
        //add to _successTime to progress subgame.
        //max time for su 1`ccesstime (when the subgame marks itself as finished) is data.TargetTime

        SuccessCheck();
        PenaltyCheck(); 
    }

    public override void StartSubgame(SubgameData data)
    {
        base.StartSubgame(data);

        ShowCam(0);

        //put code here that you want to run every time subgame is started

        InvokeRepeating(nameof(SpawnSteam), 0.1f, 0.5f);

        //audio

        //item

        CurrentCharacter.HoldItem(HeldItem.CookSpoon);

    }

    protected override void Initialize()
    {
        base.Initialize();

        //called just one, like 'start' but for subgame
        //for example, instnaitating your sound objects

        centerX = Screen.width / 2;
        centerY = Screen.height / 2;

    }

    private void OnDisable()
    {
       ResetCam();

        foreach (SteamCloud cloud in steamclouds) { 
            if (cloud) Destroy(cloud.gameObject);
        }

        steamclouds.Clear();
        CancelInvoke(nameof(SpawnSteam));

        CurrentCharacter.ClearItem();

    }
    public void SpawnSteam()
    {
        //make cloud and add to list of clouds

        SteamCloud newCloud = Instantiate(cloudPrefab, canvasRect.transform);
        steamclouds.Add(newCloud);
        //Debug.Log(canvasRect.rect.width + " " + canvasRect.rect.height);

        //put cloud in random position

        float randomX = Random.Range(-(canvasRect.rect.width/2), (canvasRect.rect.width / 2));
        float randomY = Random.Range(-(canvasRect.rect.height/2), (canvasRect.rect.height/2));

        newCloud.GetComponent<RectTransform>().anchoredPosition = new Vector2(randomX, randomY);

        //set cloud timer
        newCloud.SetTime(Data.CloudLifetime);

    }
    public void SuccessCheck()
    {
        foreach (SteamCloud cloud in steamclouds)
        {
            if (cloud != null && cloud.clicked == true)
            {
                cloud.clicked = false;

                Destroy(cloud.gameObject);
                SuccessTime += Data.SteamValue;
                //add audio effect

            }
        }
    }
    public void PenaltyCheck()
    {
        foreach(SteamCloud cloud in steamclouds)
        {
            if(cloud != null && cloud.tooLate == true)
            {
                SuccessTime -= Data.SteamPenalty;
                Destroy(cloud.gameObject);
                //add audio effect
            }
        }
    }


}
