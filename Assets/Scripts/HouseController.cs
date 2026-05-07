using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseController : MonoBehaviour
{
    [SerializeField] private Image _characterIcon;
    [SerializeField] private HappinessBar _happinessBar;
    [SerializeField] private Animator _portraitParent;
    [SerializeField] private GameObject _ruinedHouse;
    [SerializeField] private GameObject _houseModel;
    [SerializeField] private ColorApplier _mainHouseColor;
    [SerializeField] private Animator _houseAnimator;

    [SerializeField, ReadOnly] private string favColor;
    [SerializeField, ReadOnly] private Color favColorColor;
    private ID _id;
    private NeighborhoodController _controller;
    private float timeWhenEnabled = 0;
    private bool _hovered;

    private void OnEnable()
    {
        timeWhenEnabled = Time.time;

        var rot = _ruinedHouse.transform.localEulerAngles;
        rot.y = Random.Range(0, 360);
        _ruinedHouse.transform.localEulerAngles = rot;
    }

    private void Update()
    {
        UpdateHovered();
        if (_hovered) _portraitParent.gameObject.SetActive(true);

        if (_hovered && Input.GetMouseButtonUp(0)) ShowRoom();
    }

    private async void ShowRoom()
    {
        await TownGameManager.i.FadeScreen(true);
        _controller.ShowRoom(_id);
        await TownGameManager.i.FadeScreen(false);
    }

    public void Hide()
    {
        gameObject.name = "ruined house";
        _ruinedHouse.SetActive(true);
        _houseModel.SetActive(false);
    }

    public void Initialize(ID id, NeighborhoodController controller)
    {
        var character = CharacterManager.i.GetNameFormatted(id);
        gameObject.name = character + "'s house";

        _happinessBar.Initialize(id);

        favColor = CharacterManager.i.GetFavoriteColor(id).ToString();
        favColorColor = CharacterManager.i.GetClothingColor(id);

        _controller = controller;
        gameObject.SetActive(true);
        _id = id;
        _characterIcon.sprite = CharacterManager.i.GetPortrait(id);

        _ruinedHouse.SetActive(false);
        _houseModel.SetActive(true);

        _mainHouseColor.ApplyColor(CharacterManager.i.GetClothingColor(id));

        _houseAnimator.SetBool("HasProblem", CharacterManager.i.GetProblem(id) != null);
    }

    private void UpdateHovered()
    {
        var oldHovered = _hovered;

        if (Time.time - timeWhenEnabled < 1) _hovered = false;
        else {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var didHit = Physics.Raycast(ray, out var hitInfo);
            if (!didHit) _hovered = false;
            else _hovered = hitInfo.collider.GetComponentInParent<HouseController>() == this;
        }

        if (!_hovered && oldHovered) _portraitParent.SetTrigger("Exit");
    }
}