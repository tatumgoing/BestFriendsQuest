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
    public GameObject minigameNavMenu;

    [Header("Rewards Animation")]
    public HappinessBar rewardsHappinessMeter;
    public GameObject currencyDisplay;

    private void Start()
    {
        rewardsHappinessMeter.associatedCharacter = associatedCharacter;

        minigameNavMenu.SetActive(false);

        rewardsHappinessMeter.gameObject.SetActive(false);
    }


    public void SetHouseCharacter(CompleteCharacterData character)
    {
        associatedCharacter = character;
    }

    public async void ShowMinigameOptions()
    {
        if (associatedCharacter.HasProblem && associatedCharacter.CurrentProblem.Type == ProblemType.MINIGAME)
        {
            await Task.Delay(3000);

            minigameNavMenu.SetActive(true);
        }
    }

    public void NotNow()
    {
        minigameNavMenu.SetActive(false);
    }

    public void StartProblemMinigame()
    {
        TownGameManager.i.ChangeScene(TownGameManager.i.minigameUI);
        MinigameManager.i.StartProblemMinigame(associatedCharacter);
        SolveProblemInHouse();
    }

    public IEnumerator SolveProblem()
    {
        yield return new WaitForSeconds(.5f);

        //dialogueBox.DisplayDialogue("Wow! That was exactly what I was looking for!");

        yield return new WaitForSeconds(4f);

        //dialogueBox.HideDialogue();
        StartCoroutine(RewardsAnimation(associatedCharacter.CurrentProblem.RewardHappiness, associatedCharacter.CurrentProblem.RewardCurrency));
    }

    public IEnumerator FailProblem()
    {
        yield return new WaitForSeconds(.5f);

        //dialogueBox.DisplayDialogue("Oh... Thanks, I guess?");

        yield return new WaitForSeconds(4f);

        //dialogueBox.HideDialogue();

        StartCoroutine(RewardsAnimation(-10f, 1f));


    }

    public IEnumerator RecieveGift()
    {
        yield return new WaitForSeconds(.5f);

        //dialogueBox.DisplayDialogue("For me? You shouldn't have!");

        yield return new WaitForSeconds(4f);

        //dialogueBox.HideDialogue();

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
        if (associatedCharacter.HasProblem && associatedCharacter.CurrentProblem.Type != ProblemType.MINIGAME)
        {
            CharacterManager.i.SolveAndGenerateProblem(associatedCharacter.ID);
        }
    }

    private void SolveProblemInHouse()
    {
        if (associatedCharacter.HasProblem)
        {
            CharacterManager.i.SolveAndGenerateProblem(associatedCharacter.ID);
        }
    }
}
