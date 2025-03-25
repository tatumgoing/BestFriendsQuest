using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StirMinigame : MonoBehaviour
{
    public MinigameManager manager;

    public List<TargetZone> targets = new List<TargetZone>();

    public GameObject tempIcon;

    [Header("Scoring")]

    public float addScore;
    public float penaltyScore;

    [Header("Icon")]
    public GameObject barIcon;
    float iconHeight;
    float iconWidth;

    [Header("Animation")]
    public float upperSpeed;
    public float lowerSpeed;

    public float minSpeedBlackout;
    public float maxSpeedBlackout;

    [Header("Audio")]
    [SerializeField] private Sound stirringSFX;

    void Start()
    {
        stirringSFX = Instantiate(stirringSFX);
        stirringSFX.Play();

        iconHeight = barIcon.GetComponent<RectTransform>().sizeDelta.y;
        iconWidth = barIcon.GetComponent<RectTransform>().sizeDelta.x;

        //BAD BAD BAD BAD BAD KILL
        manager = FindFirstObjectByType<MinigameManager>();

        InvokeRepeating("GenerateNewSpeed", 5.0f, 4.0f);

    }
    void Update()
    {
        CheckTargets();

        if (manager.currentTimer != null)
        {
            if (CheckTargets())
            {
                manager.currentTimer.AddProgress(addScore * Time.deltaTime);
                stirringSFX.SetPercentVolume(100, 10 * Time.deltaTime);

            }
            else
            {
                manager.currentTimer.RemoveProgress(penaltyScore * Time.deltaTime);
                stirringSFX.SetPercentVolume(0, 10 * Time.deltaTime);

            }
        }
        if (!manager.currentTimer.timerActive)
        {
            ChangeSpeed(0);
        }

    }

    public void GenerateNewSpeed()
    {
        float randomSpeed = Random.Range(lowerSpeed, upperSpeed);

        ChangeSpeed(randomSpeed);

        if (minSpeedBlackout < randomSpeed && randomSpeed < maxSpeedBlackout)
        {
            GenerateNewSpeed();
        }

    }
    public void ChangeSpeed(float newSpeed)
    {
        CircleMovement barMovement= barIcon.GetComponent<CircleMovement>();

        barMovement.moveSpeed = newSpeed;
    }

    public bool CheckTargets()
    {
        bool inRange = false;

        float xMouse= Input.mousePosition.x;
        float yMouse = Input.mousePosition.y;

        float xBar = barIcon.GetComponent<RectTransform>().position.x;
        float yBar = barIcon.GetComponent<RectTransform>().position.y;

        if (xBar - iconWidth/2 <= xMouse && xBar + iconWidth / 2 >=  xMouse)
        {

            if (yBar - iconHeight / 2 <= yMouse && yBar + iconHeight / 2 >= yMouse)
            {
                inRange = true;
            }
        }
        //Debug.Log("MOUSE POSITION: " + xMouse + " " + yMouse + "\n BAR POSITION: " + xBar + " " + yBar + inRange);

        return inRange;

    }

}
