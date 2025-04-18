using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterHouse : MonoBehaviour
{
    public TownGameManager gameManager;

    [Header("Character Info")]

    public CharacterData associatedCharacter;
    public Image tempIcon;

    [Header("Dialogue Box")]

    public CharacterDialogue dialogueBox;


    [Header("Gift Inventory")]
    public GameObject giftMenu;

    public GameObject giftButton;
    public TMP_Text giftButtonText;


    private bool giftEnabled = false;

    public RecordsManager giftManager;
    //public ItemBanner selectedGift;

    [Header("Status Menu")]

    public GameObject houseStatusMenu;

    public GameObject statusButton;

    private bool statusEnabled = false;
    public TMP_Text displayName;
    public TMP_Text statusButtonText;

    public HappinessBar statusHappinessMeter;

    public GameObject relationshipPrefab;
    public GameObject relationshipContainer;

    [Header("Rewards Animation")]

    public HappinessBar rewardsHappinessMeter;
    public GameObject currencyDisplay;
   




    private void Start()
    {
        //set associated character lol
        rewardsHappinessMeter.associatedCharacter = associatedCharacter;
        statusHappinessMeter.associatedCharacter = associatedCharacter;

        dialogueBox.associatedCharacter = associatedCharacter;
        tempIcon.sprite = associatedCharacter.characterIcon;

        giftMenu.SetActive(false);
        houseStatusMenu.SetActive(false);

        rewardsHappinessMeter.gameObject.SetActive(false);

        gameManager = TownGameManager.i;

    }

    private void OnEnable()
    {
        statusEnabled = false;
        houseStatusMenu.SetActive(false);

        UpdateRelationships();

        displayName.text = associatedCharacter.characterName;
    }

    public void SetHouseCharacter(CharacterData character)
    {
        associatedCharacter = character;
    }

    public void ToggleStatusWindow()
    {
        if (statusEnabled)
        {
            houseStatusMenu.SetActive(false);
            statusEnabled = false;
            statusButtonText.text = "Status";
            
            giftButton.SetActive(true);

        }
        else
        {
            houseStatusMenu.SetActive(true); 
            statusEnabled = true;
            statusButtonText.text = "X";
            
            giftButton.SetActive(false);
        }
    }

    public void ToggleGiftWindow()
    {
        if (giftEnabled)
        {
            giftMenu.SetActive(false);
            giftEnabled = false;
            giftButtonText.text = "Gift";

            statusButton.SetActive(true);
        }
        else
        {
            giftMenu.SetActive(true);
            giftEnabled = true;
            giftButtonText.text = "X";

            statusButton.SetActive(false);
        }

    }
    

    public void UpdateRelationships()
    {
        foreach (CharacterData reloCharacter in associatedCharacter.relationships.Keys)
        {
            RelationshipBanner newBanner = Instantiate(relationshipPrefab, relationshipContainer.transform).GetComponent<RelationshipBanner>();
            newBanner.icon.sprite = reloCharacter.characterIcon;
            newBanner.nameRelo.text = reloCharacter.characterName;
            newBanner.level.text = associatedCharacter.relationships[reloCharacter].ToString();
            newBanner.status.text = "Testing";
        } 
    }

   
    public void GiveGift()
    {
        if (giftManager.selectedBanner != null)
        {
            ToggleGiftWindow();
            dialogueBox.HideDialogue();

            if (associatedCharacter.hasProblem)
            {
              
                if (associatedCharacter.currentProblem.desiredItem.Name == giftManager.selectedBanner.itemID.Name)
                {
                    StartCoroutine(SolveProblem());
                }
                else
                {
                    StartCoroutine(FailProblem());
                }
            }
            else
            {
                StartCoroutine(RecieveGift());
            }
        }
    }

    public IEnumerator SolveProblem()
    {
        yield return new WaitForSeconds(.5f);

        dialogueBox.DisplayDialogue("Wow! That was exactly what I was looking for!");

        yield return new WaitForSeconds(4f);

        dialogueBox.HideDialogue();
        StartCoroutine(RewardsAnimation(associatedCharacter.currentProblem.rewardHappiness, associatedCharacter.currentProblem.rewardCurrency));

    }

    public IEnumerator FailProblem()
    {
        yield return new WaitForSeconds(.5f);

        dialogueBox.DisplayDialogue("Oh... Thanks, I guess?");

        yield return new WaitForSeconds(4f);

        dialogueBox.HideDialogue();

        StartCoroutine(RewardsAnimation(-10f, 1f));


    }

    public IEnumerator RecieveGift()
    {
        yield return new WaitForSeconds(.5f);

        dialogueBox.DisplayDialogue("For me? You shouldn't have!");

        yield return new WaitForSeconds(4f);

        dialogueBox.HideDialogue();

        StartCoroutine(RewardsAnimation(15f, 5f));

    }

    public IEnumerator RewardsAnimation(float rHappiness, float rCurrency)
    {

        //happiness anim

        yield return new WaitForSeconds(.5f);

        rewardsHappinessMeter.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        associatedCharacter.happiness += rHappiness;
        associatedCharacter.happiness = Mathf.Clamp(associatedCharacter.happiness, 0, 100);

        yield return new WaitForSeconds(1f);

        //currency anim

        rewardsHappinessMeter.gameObject.SetActive(false);
        currencyDisplay.SetActive(true);

        yield return new WaitForSeconds(1f);

        gameManager.currency += rCurrency;

        yield return new WaitForSeconds(1f);

        currencyDisplay.SetActive(false);


        //stop old problem, make new problem

        if (associatedCharacter.hasProblem)
        {
            associatedCharacter.hasProblem = false;
            associatedCharacter.currentProblem = null;

            gameManager.GenerateProblem(associatedCharacter);
        }


    }
}
