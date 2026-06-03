using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameResultsScreen : MonoBehaviour
{
    [SerializeField] private Transform _listParent;
    [SerializeField] private float _delay;

    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private float _letterDelay = 0.075f;
    [SerializeField] private MinigameResultsListEntry _scoreEntry;
    [SerializeField] private MinigameResultsListEntry _rewardEntry;
    [SerializeField] private MinigameResultsListEntry _chefEntry;
    [SerializeField] private MinigameResultsListEntry _recipientEntry;
    [SerializeField] private MinigameResultsListEntry _relationshipEntry;
    [SerializeField] private GameObject _startCutsceneButton;
    [SerializeField] private GameObject _normalButtonParent;

    [Header("Cutscenes")]
    [SerializeField] private TextAsset _cutsceneScript;

    private ID _chef;
    private ID _recipient;
    private bool _animationDone; 
    private bool _cutscenePlayed;

    public void CompleteAnimation() => _animationDone = true;

    public void showResultsCutscene()
    {
        gameObject.SetActive(false);
        CutsceneManager.i.StartCutscene(_cutsceneScript, new List<ID> { _chef, _recipient}, () => gameObject.SetActive(true));
        _cutscenePlayed = true;
    }

    private void OnEnable()
    {
        _animationDone = false;
        _normalButtonParent.SetActive(false);

        if (_cutscenePlayed) {
            _cutscenePlayed = false;
            _startCutsceneButton.SetActive(false);
            ShowNormalButtons();
        }
    }

    private async void ShowNormalButtons()
    {
        while (!_animationDone) await Task.Yield(); //AnimationDone is set to true by an animation event at the end of the animation via 'CompleteAnimation()' function
        _normalButtonParent.SetActive(true);
    }

    public async Task ShowScore(float finalScore, RecipeData recipe, ID chef, ID recipient, bool isProblem, float originalHappinessChef, float originalHappinessRecipient)
    {
        _chef = chef;
        _recipient = recipient;
        var intDelay = Mathf.CeilToInt(_delay * 1000);

        _titleText.text = "";

        foreach (Transform child in _listParent) child.gameObject.SetActive(false);
        _startCutsceneButton.SetActive(false);

        gameObject.SetActive(true);
        _animationDone = false;
        while (!_animationDone) await Task.Yield(); //AnimationDone is set to true by an animation event at the end of the animation via 'CompleteAnimation()' function

        var titleText = "Dish Complete";
        for (int i = 0; i < titleText.Length; i++) {
            _titleText.text += titleText[i];
            await Task.Delay(Mathf.CeilToInt(_letterDelay * 1000));
        }
        await Task.Delay(intDelay/2);

        _scoreEntry.Initialize(finalScore);
        await Task.Delay(intDelay);
        
        _rewardEntry.Initialize(Mathf.CeilToInt(finalScore * recipe.MoneyReward));
        await Task.Delay(intDelay);

        _chefEntry.gameObject.SetActive(recipient == chef);
        if (chef == recipient) {
            var happinessDelta = Mathf.Min(recipe.HappinessReward * finalScore, 100 - originalHappinessChef);
            _chefEntry.Initialize(chef, happinessDelta, _delay * 0.75f);
            await Task.Delay(intDelay);
        }

        _recipientEntry.gameObject.SetActive(recipient != chef);
        _relationshipEntry.gameObject.SetActive(false);
        if (recipient != chef) {
            var happinessDelta = Mathf.Min(recipe.HappinessReward * finalScore, 100 - originalHappinessRecipient);
            _recipientEntry.Initialize(recipient, happinessDelta, _delay * 0.75f);
            await Task.Delay(intDelay);

            _relationshipEntry.gameObject.SetActive(true);
            var relationshipDelta = recipe.RelationshipReward * finalScore;
            var originalRelationship = CharacterManager.i.GetRelationship(chef, recipient) - relationshipDelta;
            _relationshipEntry.Initialize(chef, recipient, originalRelationship, relationshipDelta, _delay * 0.75f);
        }

        await Task.Delay(intDelay/2);

        _startCutsceneButton.SetActive(true);
    }

    public void ReturnToProblemRoom()
    {
        GetComponentInParent<MinigameController>().CompleteProblem();
    }
}
