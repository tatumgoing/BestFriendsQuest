using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChopMinigame : MonoBehaviour
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
    public float barSpeed; // or period
    public float amplitude; // one half width of bar
    public float shift; //should be the middle of the bar

    [Header("Sounds")]
    [SerializeField] private Sound chopSFX;




    // Start is called before the first frame update
    void Start()
    {
        //create sounds
        chopSFX = Instantiate(chopSFX);


        //BAD BAD BAD BAD BAD KILL
        manager= FindFirstObjectByType<MinigameManager>();


        iconPosX = barIcon.GetComponent<RectTransform>().anchoredPosition.x;
        iconPosY = barIcon.GetComponent<RectTransform>().anchoredPosition.y;


      
         amplitude = cookingBar.GetComponent<RectTransform>().sizeDelta.x /2 ;
         shift = cookingBar.GetComponent<RectTransform>().anchoredPosition.x;

        

        foreach (TargetZone target in cookingBar.GetComponentsInChildren<TargetZone>()) { 
            targets.Add(target);
            target.SetBounds(target.GetComponent<RectTransform>().anchoredPosition.x);
        }

        tempIcon.GetComponent<Image>().sprite = manager.characterSelectionMenu.selectedCharacter.characterIcon;

    }

    // Update is called once per frame
    void Update()
    {

        if (manager.currentTimer.timerActive) {
            iconPosX = (amplitude * Mathf.Sin(barSpeed * (Time.time)) + shift);
            barIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(iconPosX, iconPosY);
        }

        if (Input.GetKeyDown("space") || Input.GetMouseButtonDown(0))
        {
            if (CheckTargets())
            {
                manager.currentTimer.AddProgress(addScore); 
                chopSFX.Play(oneShot:true);
            }
            else
            {
                manager.currentTimer.RemoveProgress(penaltyScore);
            }

            //Debug.Log(CheckTargets());
        }
    }


    public bool CheckTargets()
    {
        bool inRange = false;
        foreach (TargetZone target in targets)
        {
            
            if (iconPosX >= target.lowerBound && iconPosX <= target.upperBound)
                {
                    inRange = true;
                }
            
        }

        //Debug.Log(inRange);

        return inRange;
    }


}
