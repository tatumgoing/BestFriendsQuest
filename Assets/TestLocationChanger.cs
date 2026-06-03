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

    private void GoTo(AreaName target)
    {
        _ = GetComponent<TownGameManager>().ChangeArea(target);
    }
}
