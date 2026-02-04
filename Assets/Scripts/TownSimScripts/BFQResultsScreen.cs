using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

public class BFQResultsScreen : MonoBehaviour
{
    Quest associatedQuest;

    [Header("Characters")]
    public CompleteCharacterData charOne;
    public CompleteCharacterData charTwo;

    [Header("Sprites")]
    public Image charDisplayOne;
    public Image charDisplayTwo;
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

    public RelationshipBar relationBar;

    void OnEnable()
    {
        resultsScreen.SetActive(false);

        successWindow.SetActive(false);
        failWindow.SetActive(false);
        statsWindow.SetActive(false);
    }
    public IEnumerator ResultsAnimation(bool succeeded, Quest newQuest, CompleteCharacterData cOne, CompleteCharacterData cTwo)
    {
        associatedQuest = newQuest;

        charOne = cOne;
        charTwo = cTwo;

        charDisplayOne.sprite = charOne.Icon;
        charDisplayTwo.sprite = charTwo.Icon;

        relationBar.SetCharacters(charOne.ID, charTwo.ID);

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


        if (succeeded) StartCoroutine(SuccessAnimation());
        else StartCoroutine(FailAnimation());
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

        StartCoroutine(StatsAnimation("Friendship strengthened!", true));


    }

    public IEnumerator FailAnimation()
    {
        resultsScreen.SetActive(true);

        topText.text = "Too bad...";
        failWindow.SetActive(true);
        failWindow.GetComponent<Image>().sprite = associatedQuest.unlockedItem.sprite;

        yield return new WaitForSeconds(5f);

        StartCoroutine(StatsAnimation("Friendship weakened...", false));
    }

    public IEnumerator StatsAnimation(string displayText, bool success)
    {
        failWindow.SetActive(false);
        successWindow.SetActive(false);

        statsWindow.SetActive(true);

        statsText.text = displayText;
            
        resCharOne.sprite= charOne.Icon;
        resCharTwo.sprite= charTwo.Icon;

        yield return new WaitForSeconds(3f);

        var relationshipChange = success ? associatedQuest.relationshipGain : associatedQuest.relationshipLoss;
        CharacterManager.i.IncreaseRelationship(charOne.ID, charTwo.ID, relationshipChange);

        charOne.SetHappiness(0);
        charTwo.SetHappiness(0);
    }
}
