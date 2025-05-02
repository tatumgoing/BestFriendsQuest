using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

public class BFQResultsScreen : MonoBehaviour
{
    Quest associatedQuest;

    [Header("Sprites")]
    public Image charOne;
    public Image charTwo;
    public Image treasureChest;

    public Sprite chestClosed;
    public Sprite chestOpen;

    public Image resCharOne;
    public Image resCharTwo;

    [Header("Animation")]

    public GameObject transitionScreen;
    public GameObject resultsScreen;

    [Header("Final Screen")]

    public TMP_Text topText;
    public GameObject successWindow;
    public TMP_Text successText;
    public GameObject failWindow;
    public GameObject statsWindow;
    public TMP_Text statsText;

    void OnEnable()
    {
        resultsScreen.SetActive(false);

        successWindow.SetActive(false);
        failWindow.SetActive(false);
        statsWindow.SetActive(false);
    }
    public IEnumerator ResultsAnimation(bool succeeded, Quest newQuest)
    {
        associatedQuest = newQuest;

        FunAnimator anim = treasureChest.gameObject.GetComponent<FunAnimator>();
        anim.doesRot = false;
        var rotSpeed = anim.rotSpeed;

        yield return new WaitForSeconds(3f);

        anim.doesRot = true;
        anim.rotSpeed = rotSpeed*4;

        yield return new WaitForSeconds(1f);

        treasureChest.sprite = chestOpen;
        anim.rotSpeed = rotSpeed;

        yield return new WaitForSeconds(2f);


        if (succeeded)
        {
            StartCoroutine(SuccessAnimation());
        }
        else
        {
            StartCoroutine(FailAnimation());
        }

    }

    public IEnumerator SuccessAnimation()
    {
        resultsScreen.SetActive(true);

        topText.text = "Congratulations!";
        successWindow.SetActive(true);
        successText.text = "Your townsfolk brought back " + associatedQuest.unlockedItem.Name + "!";
        successWindow.GetComponent<Image>().sprite = associatedQuest.unlockedItem.sprite;

        associatedQuest.unlockedItem.unlocked = true;

        yield return new WaitForSeconds(5f);

        StartCoroutine(StatsAnimation("Friendship strengthened!"));


    }

    public IEnumerator FailAnimation()
    {
        resultsScreen.SetActive(true);

        topText.text = "Too bad...";
        failWindow.SetActive(true);
        failWindow.GetComponent<Image>().sprite = associatedQuest.unlockedItem.sprite;

        yield return new WaitForSeconds(5f);

        StartCoroutine(StatsAnimation("Friendship weakened..."));
    }

    public IEnumerator StatsAnimation(string displayText)
    {
        failWindow.SetActive(false);
        successWindow.SetActive(false);

        statsWindow.SetActive(true);

        statsText.text = displayText;
            
        resCharOne.sprite= charOne.sprite;
        resCharTwo.sprite= charTwo.sprite;

        yield return new WaitForSeconds(5f);

    }


}
