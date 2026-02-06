using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborhoodUI : MonoBehaviour
{
    [SerializeField] private GameObject _roomUI;
    [SerializeField] private GameObject _neighborhoodUI;
    [SerializeField] private NeighborhoodController _controller;

    public void ExitNeighborhood()
    {
        TownGameManager.i.GoToMap();
    }

    public void ShowRoomUI()
    {
        gameObject.SetActive(true);

        _roomUI.SetActive(true);
        _neighborhoodUI.SetActive(false);
    }

    public void ShowNeighborhoodUI()
    {
        _neighborhoodUI.SetActive(true);
        _roomUI.SetActive(false);
        gameObject.SetActive(true);
    }

    public async void HideRoomUI()
    {
        await TownGameManager.i.FadeScreen(true);
        _roomUI.SetActive(false);
        _controller.LeaveRoom();
        ShowNeighborhoodUI();
        await TownGameManager.i.FadeScreen(false);
    }
}
