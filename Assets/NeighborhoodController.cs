using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NeighborhoodController : MonoBehaviour
{
    [SerializeField] private CharacterRoomModel _room;
    [SerializeField] private GameObject _neighborhoodCamera;

    private void OnEnable()
    {
        HouseCharacters();
        TownUIManager.i.ShowNeighborhoodUI();
    }

    public void LeaveRoom()
    {
        _room.Hide();
        _neighborhoodCamera.SetActive(true);
    }

    public void ShowRoom(ID id)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        _room.Show(id);
        _neighborhoodCamera.SetActive(false);

        TownUIManager.i.ShowRoomUI(id);
    }

    private void HouseCharacters()
    {
        var houses = GetComponentsInChildren<HouseController>();
        houses.Shuffle();

        var characters = CharacterManager.i.AllIDs();

        for (int i = 0; i < houses.Length; i++) {
            if (i >= characters.Count) houses[i].Hide();
            else houses[i].Initialize(characters[i], this);
        }

        _room.Hide();
        _neighborhoodCamera.SetActive(true);
    }
}
