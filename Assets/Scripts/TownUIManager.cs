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

    public void ShowNeighborhoodUI() => _neighborhood.ShowNeighborhoodUI();
    public void ShowRoomUI(ID id) => _neighborhood.ShowRoomUI(id);
}
