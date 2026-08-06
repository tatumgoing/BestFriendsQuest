using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class FocusedHouseUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private GameObject _enterButton;
    [SerializeField] private GameObject _summonButton;
    [SerializeField] private float _letterDelayTime = 0.01f;
    [SerializeField] private Sound _letterSound;
    [SerializeField] private GameObject _mapButton;

    private int _targetVisibleCharacters;
    private float _letterCooldown;
    private ID _id;

    private void Start()
    {
        _letterSound = Instantiate(_letterSound);
    }

    private void Update()
    {
        _letterCooldown -= Time.deltaTime;
        if (_letterCooldown <= 0 && _targetVisibleCharacters > _descriptionText.maxVisibleCharacters) {
            _descriptionText.maxVisibleCharacters += 1;
            _letterSound.Play(restart: false);
            _letterCooldown = _letterDelayTime;
        }
    }

    private void OnDisable()
    {
        _mapButton.SetActive(true);
    }

    public void Show(ID id)
    {
        _id = id;
        gameObject.SetActive(true);

        _descriptionText.maxVisibleCharacters = 0;
        _descriptionText.text = "";
        _targetVisibleCharacters = 0;

        _enterButton.gameObject.SetActive(false);
        _summonButton.gameObject.SetActive(false); 

        if (id == new ID(0)) {
            _titleText.text = "Vacant House";
            _descriptionText.text = "No one lives here! Invite new townsfolk from the town hall.";
            return;
        }

        var name = CharacterManager.i.GetNameFormatted(id);
        _titleText.text = name + "'s House";

        var isHere = CharacterManager.i.GetIDsByArea(AreaName.TOWN).Contains(id);
        if (!isHere) _descriptionText.text = name + " is " + CharacterManager.i.GetLocation(id);
        else _descriptionText.text = GetDescription(id);

        _enterButton.gameObject.SetActive(isHere);
        _summonButton.gameObject.SetActive(!isHere);
    }

    private string GetDescription(ID id)
    {
        var name = CharacterManager.i.GetNameFormatted(id);
        var res = name;
        var time = System.DateTime.Now;
        if (time.Hour < 10) {
            if (id % 3 == 0) res += " is sleeping in.";
            else res += " is having breakfast.";
        }
        else if (time.Hour < 14) {
            if (id % 3 == 0) res += " is having a nice lazy day at home";
            res += " is eating lunch.";
        }
        else if (time.Hour > 18) {
            if (id % 3 == 0) res += " is having dinner.";
            else res += " is relaxing at home.";
        }
        else res += " is fast asleep.";

        return res;
    }

    public void CallHome()
    {
        CharacterManager.i.ChangeCharacterLocation(_id, AreaName.TOWN);
        _descriptionText.text = GetDescription(_id);
        _descriptionText.maxVisibleCharacters = 0;
        _targetVisibleCharacters = _descriptionText.text.Length;

        _summonButton.SetActive(false);
        _enterButton.SetActive(true);
    }

    public void OnAnimationEnd()
    {
        _targetVisibleCharacters = _descriptionText.text.Length;
    }

    public async void ShowRoom()
    {
        await TownGameManager.i.FadeScreen(true);
        gameObject.SetActive(false);
        GetComponentInParent<NeighborhoodUI>().ShowRoomUI(_id);
        await TownGameManager.i.FadeScreen(false);
    }
}
