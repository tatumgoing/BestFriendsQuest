using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class NeighborhoodController : MonoBehaviour
{
    [SerializeField] private CharacterRoomModel _room;
    [SerializeField] private GameObject _neighborhoodCamera;
    [SerializeField] private List<CharacterSpawnLocation> _spawnSpots;
    [SerializeField] private CharacterPointWalker _walker;

    private List<GameObject> _spawnedCharacters = new List<GameObject>();

    private void OnEnable()
    {
        if (!TownUIManager.i) return;

        HouseCharacters();
        TownUIManager.i.ShowNeighborhoodUI();

        foreach (var c in _spawnedCharacters) if (c) Destroy(c);
        _spawnedCharacters.Clear();

        var currentCharacters = CharacterManager.i.GetIDsByArea(AreaName.TOWN);
        currentCharacters.Shuffle();
        _spawnSpots.Shuffle();

        for (int i = 0; i < currentCharacters.Count; i++) {
            if (i >= _spawnSpots.Count) break;
            var newCharacter = CharacterManager.i.SpawnCharacter(currentCharacters[i], _spawnSpots[i].transform);
            _ = _spawnSpots[i].SetCharacter(newCharacter);
            _spawnedCharacters.Add(newCharacter.gameObject);

            _walker.AddWalker(newCharacter.transform);
        }
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
        //houses.Shuffle();

        var characters = CharacterManager.i.AllIDs().OrderBy(x => (int)x).ToList();

        for (int i = 0; i < houses.Length; i++) {
            if (i >= characters.Count) houses[i].Hide();
            else houses[i].Initialize(characters[i], this);
        }

        _room.Hide();
        _neighborhoodCamera.SetActive(true);
    }
}
