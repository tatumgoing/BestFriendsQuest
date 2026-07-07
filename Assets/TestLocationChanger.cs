using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLocationChanger : MonoBehaviour
{

    [ButtonMethod] private void GoToResturaunt() => GoTo(AreaName.RESTURAUNT);
    [ButtonMethod] private void GoToPark() => GoTo(AreaName.PARK);
    [ButtonMethod] private void GoToTown() => GoTo(AreaName.TOWN);
    [ButtonMethod] private void GoToPort() => GoTo(AreaName.PORT);
    [ButtonMethod] private void GoToGrocery() => GoTo(AreaName.GROCERY_STORE);
    [ButtonMethod] private void GoToTownHall() => GoTo(AreaName.TOWN_HALL);

    private void GoTo(AreaName target)
    {
        _ = GetComponent<TownGameManager>().ChangeArea(target);
    }
}
