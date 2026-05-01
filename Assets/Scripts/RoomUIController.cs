using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private CharacterStatusMenu _statusMenu;
    [SerializeField] private CharacterDialogue _dialogue;
    [SerializeField] private GiftMenu _giftMenu;
    [SerializeField] private GameObject _talkOnlyButtonParent;
    [SerializeField] private GameObject _buttonsParent;
    [SerializeField] private GameObject _backButton;
    [SerializeField] private ProblemRewardsDisplay _problemRewardsDisplay;

    private bool _justGaveGift;
    private ID _id;

    public void GiveGift()
    {
        _justGaveGift = true;
        Show(_id);

        Talk();
    }

    public void Show(ID id)
    {
        _id = id;
        var character = CharacterManager.i.GetCharacter(id);
        _nameText.text = character.Name + "'s";

        _statusMenu.gameObject.SetActive(false);
        _dialogue.gameObject.SetActive(false);

        var problem = CharacterManager.i.GetProblem(id);
        var hasSolvedProblem = problem && problem.IsSolved;
        var forceTalk = hasSolvedProblem || _justGaveGift;

        _talkOnlyButtonParent.SetActive(forceTalk);
        _buttonsParent.SetActive(!forceTalk);
        _backButton.SetActive(!forceTalk);

        if (hasSolvedProblem) _problemRewardsDisplay.Show(problem);
        else _problemRewardsDisplay.gameObject.SetActive(false);

        _giftMenu.gameObject.SetActive(false);

        gameObject.SetActive(true);
    }

    public void ShowStatus()
    {
        _statusMenu.Show(_id);
    }

    public void Talk()
    {
        _dialogue.Talk(_id, _justGaveGift);
        _justGaveGift = false;
    }

    public void ShowGiftMenu()
    {
        _giftMenu.Show(_id);
    }

    public void HideRoom()
    {
        GetComponentInParent<NeighborhoodUI>().HideRoomUI();
    }

}
