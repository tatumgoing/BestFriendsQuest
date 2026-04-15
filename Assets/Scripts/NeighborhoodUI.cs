using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborhoodUI : MonoBehaviour
{
    [SerializeField] private RoomUIController _roomUI;
    [SerializeField] private GameObject _neighborhoodUI;
    [SerializeField] private NeighborhoodController _controller;

    public void ExitNeighborhood()
    {
        TownNavigator.GoToMap();
    }

    public void ShowRoomUI(ID id)
    {
        gameObject.SetActive(true);

        _roomUI.Show(id);
        _neighborhoodUI.SetActive(false);
    }

    public void ShowNeighborhoodUI()
    {
        _neighborhoodUI.SetActive(true);
        _roomUI.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public async void HideRoomUI()
    {
        await TownGameManager.i.FadeScreen(true);
        _roomUI.gameObject.SetActive(false);
        _controller.LeaveRoom();
        ShowNeighborhoodUI();
        await TownGameManager.i.FadeScreen(false);
    }
}
