using UnityEngine;

public class TownNavigator : MonoBehaviour
{

    public static async void GoToMap() => await TownGameManager.i.ChangeArea(AreaName.MAP);
    public static async void GoToPark() => await TownGameManager.i.ChangeArea(AreaName.PARK);
    public static async void GoToTown() => await TownGameManager.i.ChangeArea(AreaName.TOWN);
    public static async void GoToShop() => await TownGameManager.i.ChangeArea(AreaName.SHOP);
    public static async void GoToResturaunt() => await TownGameManager.i.ChangeArea(AreaName.RESTURAUNT);
    public static async void GoToTownHall() => await TownGameManager.i.ChangeArea(AreaName.TOWN_HALL);
    public static async void GoToPort() => await TownGameManager.i.ChangeArea(AreaName.PORT);
    public static async void GoToRecords() => await TownGameManager.i.ChangeArea(AreaName.RECORDS);
}
