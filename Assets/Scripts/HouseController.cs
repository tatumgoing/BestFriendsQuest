using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseController : MonoBehaviour
{
    [SerializeField] private Image _characterIcon;
    [SerializeField] private Animator _portraitParent;
    
    private ID _id;
    private NeighborhoodController _controller;
    private float timeWhenEnabled = 0;
    private bool _hovered;

    private void OnEnable()
    {
        timeWhenEnabled = Time.time;
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
        gameObject.SetActive(false);
    }

    public void Initialize(ID id, NeighborhoodController controller)
    {
        _controller = controller;
        gameObject.SetActive(true);
        _id = id;
        _characterIcon.sprite = CharacterManager.i.GetPortrait(id);
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