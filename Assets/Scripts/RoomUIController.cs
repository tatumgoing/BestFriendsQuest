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

    private ID _id;

    public void Show(ID id)
    {
        _id = id;
        var character = CharacterManager.i.GetCharacter(id);
        _nameText.text = character.Name + "'s";

        _statusMenu.gameObject.SetActive(false);
        _dialogue.gameObject.SetActive(false);

        var problem = CharacterManager.i.GetProblem(id);
        var hasSolvedProblem = problem && problem.IsSolved;
        _talkOnlyButtonParent.SetActive(hasSolvedProblem);
        _buttonsParent.SetActive(!hasSolvedProblem);
        _backButton.SetActive(!hasSolvedProblem);

        gameObject.SetActive(true);
    }

    public void ShowStatus()
    {
        _statusMenu.Show(_id);
    }

    public void Talk()
    {
        _dialogue.Talk(_id);
    }

    public void ShowGiftMenu()
    {
        _giftMenu.Show();
    }

    public void HideRoom()
    {
        GetComponentInParent<NeighborhoodUI>().HideRoomUI();
    }
}
