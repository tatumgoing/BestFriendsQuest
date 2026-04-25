using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestIsland : MonoBehaviour
{
    [SerializeField, DisplayInspector] private Quest _questData;

    private QuestMapController _controller;

    private void OnEnable()
    {
        if (!_controller) _controller = GetComponentInParent<QuestMapController>();
    }

    public void Focus()
    {
        if (_controller) _controller.FocusIsland(GetComponent<SelectableItem>(), _questData);
    }
}
