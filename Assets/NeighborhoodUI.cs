using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborhoodUI : MonoBehaviour
{
    [SerializeField] private GameObject _roomUI;
    [SerializeField] private NeighborhoodController _controller;

    public void ShowRoomUI()
    {
        _roomUI.SetActive(true);
    }

    public void HideRoomUI()
    {
        _roomUI.SetActive(false);
        _controller.LeaveRoom();
    }
}
