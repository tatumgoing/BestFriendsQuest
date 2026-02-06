using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownUIManager : MonoBehaviour
{
    [SerializeField] private NeighborhoodUI _neighborhood;

    public static TownUIManager i;

    private void Awake()
    {
        i = this;
    }

    public void ShowRoomUI() => _neighborhood.ShowRoomUI();
}
