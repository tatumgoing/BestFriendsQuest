using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;

public class CharacterHouse : MonoBehaviour
{
    [Header("Character Info")]
    public CompleteCharacterData associatedCharacter;

    [Header("Dialogue Box")]
    public CharacterDialogue dialogueBox;
    public GameObject minigameNavMenu;

    [Header("Gift Inventory")]
    public GameObject giftMenu;
    private bool giftEnabled = false;
    public RecordsManager giftManager;

    [Header("Status Menu")]
    private bool statusEnabled = false;
    public TMP_Text displayName;
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

        giftMenu.SetActive(false);
        minigameNavMenu.SetActive(false);

        rewardsHappinessMeter.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        statusEnabled = false;

        displayName.text = associatedCharacter.Name;
    }

    public void SetHouseCharacter(CompleteCharacterData character)
    {
        associatedCharacter = character;
    }

    public async void ShowMinigameOptions()
    {
        if (associatedCharacter.HasProblem && associatedCharacter.CurrentProblem.IsMinigame)
        {
            await Task.Delay(3000);

            dialogueBox.HideDialogue();
            minigameNavMenu.SetActive(true);
        }
    }

    public void NotNow()
    {
        dialogueBox.HideDialogue();
        minigameNavMenu.SetActive(false);
    }

    public void StartProblemMinigame()
    {
        TownGameManager.i.ChangeScene(TownGameManager.i.minigameUI);
        MinigameManager.i.StartProblemMinigame(associatedCharacter);
        SolveProblemInHouse();
    }


    public void ToggleGiftWindow()
    {
        if (giftEnabled)
        {
            giftMenu.SetActive(false);
            giftEnabled = false;
        }
        else
        {
            giftMenu.SetActive(true);
            giftEnabled = true;
        }
    }
    
    public void GiveGift()
    {
        if (giftManager.selectedBanner != null && giftManager.selectedBanner.itemCount.text != "0")
        {
            ToggleGiftWindow();
            dialogueBox.HideDialogue();
            bool passed = false;
            if (associatedCharacter.HasProblem && !associatedCharacter.CurrentProblem.IsMinigame) {
                foreach (Item checkItem in associatedCharacter.CurrentProblem.desiredItem) {
                    if (checkItem.Name == giftManager.selectedBanner.itemID.Name) {
                        StartCoroutine(SolveProblem());
                        passed = true;
                    }
                }
                if (!passed) {
                    StartCoroutine(FailProblem());
                }
            }
            else {
                StartCoroutine(RecieveGift());
            }

            TownGameManager.i.SubtractInventory(giftManager.selectedBanner.itemID);
            TownGameManager.i.UpdateRecordDisplay(giftManager, giftManager.currentType);
        }
    }

    public IEnumerator SolveProblem()
    {
        yield return new WaitForSeconds(.5f);

        dialogueBox.DisplayDialogue("Wow! That was exactly what I was looking for!");

        yield return new WaitForSeconds(4f);

        dialogueBox.HideDialogue();
        StartCoroutine(RewardsAnimation(associatedCharacter.CurrentProblem.rewardHappiness, associatedCharacter.CurrentProblem.rewardCurrency));
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
        //happiness anim:
        yield return new WaitForSeconds(.5f);

        rewardsHappinessMeter.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        CharacterManager.i.IncreaseHappiness(associatedCharacter.ID, rHappiness);

        yield return new WaitForSeconds(1f);

        //currency anim:

        rewardsHappinessMeter.gameObject.SetActive(false);
        currencyDisplay.SetActive(true);

        yield return new WaitForSeconds(1f);

        TownGameManager.i.currency += rCurrency;

        yield return new WaitForSeconds(1f);

        currencyDisplay.SetActive(false);


        //stop old problem, make new problem
        if (associatedCharacter.HasProblem && !associatedCharacter.CurrentProblem.IsMinigame)
        {
            CharacterManager.i.SolveProblem(associatedCharacter.ID);
            TownGameManager.i.GenerateProblem(associatedCharacter.ID);
        }
    }

    private void SolveProblemInHouse()
    {
        if (associatedCharacter.HasProblem)
        {
            CharacterManager.i.SolveProblem(associatedCharacter.ID);
            TownGameManager.i.GenerateProblem(associatedCharacter.ID);
        }
    }
}
