using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseController : MonoBehaviour
{
    [SerializeField] private Image _characterIcon;
    
    private ID _id;
    private NeighborhoodController _controller;

    private void Update()
    {
        bool hovered = IsHovered();
        _characterIcon.gameObject.SetActive(hovered);

        if (hovered && Input.GetMouseButtonDown(0)) ShowRoom();
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
        _characterIcon.sprite = CharacterManager.i.GetIcon(id);
    }

    private bool IsHovered()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var didHit = Physics.Raycast(ray, out var hitInfo);
        if (!didHit) return false;

        return hitInfo.collider.GetComponentInParent<HouseController>() == this;    
    }
}
