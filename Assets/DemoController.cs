using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class AreaUnlockedData
{
    public AreaName Area;
    public bool Unlocked;
}

public class DemoController : MonoBehaviour
{
    public static DemoController i;


    [SerializeField] private bool _forceStep0;
    [SerializeField] private bool _printSteps;
    [SerializeField] private TestResetter _resetter;


    [SerializeField] private SteamDemoUI _uiController;
    [SerializeField] private List<AreaUnlockedData> _data = new List<AreaUnlockedData>();

    [Header("Cutscenes")]
    [SerializeField] CutsceneSets _setController;
    [SerializeField] private CutsceneScript _helloImHungryScript;
    [SerializeField] private CutsceneScript _eatingFoodAloneScript;
    [SerializeField] private CutsceneScript _firstTwoMeetingScript;
    [SerializeField] private CutsceneScript _firstTwoHangingOutScript;
    [SerializeField] private CutsceneScript _secondGratefulScript;
    [SerializeField] private CutsceneScript _discoverResturauntScript;
    [SerializeField] private CutsceneScript _giveFoodToChar1Script;
    [SerializeField] private CutsceneScript _discussPortScript;
    [SerializeField] private CutsceneScript _discoverPortScript;

    [Header("Misc References")]
    [SerializeField] private DemoTutorialController _tutorialController;
    [SerializeField] private GiftMenu _giftMenu;
    [SerializeField] private GameObject _townHallUIParent;
    [SerializeField] private GameObject _shopUIParent;
    [SerializeField] private GameObject _parkUIParent;
    [SerializeField] private GameObject _resturuantUIParent;
    [SerializeField] private GameObject _portUIParent;
    [SerializeField] private CookingMinigame _cookingMinigame;

    private int _step = 0;
    private bool _shopTutorialShown = false;

    public int Step => _step;

    private void Awake()
    {
        i = this;
    }

    private void Start()
    {
        if (!TownGameManager.i.DemoMode) {
            gameObject.SetActive(false);
            return;
        }

        _step = PlayerPrefs.GetInt("DemoStep", _step);
        if (_forceStep0) {
            _resetter.ResetAll();
            ResetDemo();
        }

        if (_step == 0) DoStep0();
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("DemoStep", _step);
    }

    private void Update()
    {
        if (_step == 1 && CharacterManager.i.AllCharacters.Count > 0) DoStep1();
        if (_step == 2 && TownGameManager.i.GetInventoryItems().Count > 0) DoStep2();
        if (_step == 4 && CharacterManager.i.AllIDs().Count > 1 && _townHallUIParent.activeInHierarchy) DoStep4();
        if (_step == 5 && _parkUIParent.activeInHierarchy) DoStep5();
        if (_step == 7 && _resturuantUIParent.activeInHierarchy) DoStep7();
        if (_step == 10 && _portUIParent.activeInHierarchy) DoStep10();

        if (_step == 2 && _shopUIParent.activeInHierarchy && !_shopTutorialShown) {
            _tutorialController.ShowTutorial(DemoTutorialType.SHOP);
            _shopTutorialShown = true;
        }
    }

    private void UnlockArea(AreaName area)
    {
        foreach (var d in _data) if (d.Area == area) d.Unlocked = true;
    }

    private void AdvanceStep()
    {
        _step += 1;
        PlayerPrefs.SetInt("DemoStep", _step);
        if (_printSteps) print("triggering demo step: " + (_step-1));
    }

    [ButtonMethod]
    private void ResetDemo()
    {
        PlayerPrefs.SetInt("DemoStep", 0);
        _step = 0;
    }

    public bool IsUnlocked(AreaName area)
    {
        if (!TownGameManager.i.DemoMode) return true;

        var selected = _data.Where(x => x.Area == area).ToList();
        if (selected.Count > 0) return selected[0].Unlocked;
        return true;
    }

    private void DeleteAllSpawnedCharacters()
    {
        var allCharacters = FindObjectsByType<SpawnedCharacter>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var a in allCharacters) Destroy(a.gameObject);
    }

    [ButtonMethod]
    private void SkipToStep()
    {
        _step = 10;
        DoStep10();
    }

    private void DoStep10()
    {
        AdvanceStep();
        DeleteAllSpawnedCharacters();

        var selected = CharacterManager.i.AllIDs().Take(2).ToList();
        if (CharacterManager.i.GetRelationship(selected[0], selected[1]) < 1) CharacterManager.i.IncreaseRelationship(selected[0], selected[1], 1);

        foreach (var c in selected) CharacterManager.i.IncreaseHappiness(c, 100);

        ShowCutscene(_discoverPortScript, AreaName.PORT, selected, 1, () => {
            _ = TownGameManager.i.ChangeArea(AreaName.PORT);
            _setController.HideAll();
        });
    }

    //characters talking about the port while in town
    private void DoStep9()
    {
        AdvanceStep();
        DeleteAllSpawnedCharacters();

        var selected = CharacterManager.i.AllIDs().Take(2).ToList();

        UnlockArea(AreaName.PORT);
        ShowCutscene(_discussPortScript, AreaName.TOWN, selected, 1, () => _uiController.UnlockArea("Port"));
    }

    //char2 giving their cooked food to char1
    private void DoStep8()
    {
        _cookingMinigame.OnShowResults.RemoveListener(DoStep8);

        AdvanceStep();
        DeleteAllSpawnedCharacters();

        var selected = CharacterManager.i.AllIDs().Take(2).Reverse().ToList();
        ShowCutscene(_giveFoodToChar1Script, AreaName.TOWN, selected, 1, DoStep9);
    }

    //char2 discovering the resturaunt
    private void DoStep7()
    {
        AdvanceStep();
        DeleteAllSpawnedCharacters();

        var selected = CharacterManager.i.AllIDs()[1];
        ShowCutscene(_discoverResturauntScript, AreaName.RESTURAUNT, new List<ID>() { selected }, 1, () => {
            _ = TownGameManager.i.ChangeArea(AreaName.RESTURAUNT);
            DeleteAllSpawnedCharacters();
            _cookingMinigame.OnShowResults.AddListener(DoStep8);
        });
    }

    //character2 showing gratitude and asking to go cook
    private async void DoStep6()
    {
        await Task.Delay(100);

        AdvanceStep();
        DeleteAllSpawnedCharacters();

        var selected = CharacterManager.i.AllIDs()[1];

        UnlockArea(AreaName.RESTURAUNT);
        ShowCutscene(_secondGratefulScript, AreaName.TOWN, new List<ID>() { selected }, 1, () => _uiController.UnlockArea("Resturaunt"));
    }

    //picnic in the park!
    private void DoStep5()
    {
        AdvanceStep();
        DeleteAllSpawnedCharacters();

        var selected = CharacterManager.i.AllIDs().Take(2).ToList();

        ShowCutscene(_firstTwoHangingOutScript, AreaName.PARK, selected, 1, DoStep6);
    }

    //after the second character is created, they meet each other in the neighborhood
    private void DoStep4()
    {
        AdvanceStep();
        DeleteAllSpawnedCharacters();

        var selected = CharacterManager.i.AllIDs().Take(2).ToList();

        UnlockArea(AreaName.PARK);
        ShowCutscene(_firstTwoMeetingScript, AreaName.TOWN, selected, 1, () => _uiController.UnlockArea("Park"));
    }

    //after a character is given a food gift, cutscene of them eating it
    private void DoStep3(ID giftRecipient)
    {
        AdvanceStep();
        _giftMenu.OnGiveGift.RemoveListener(DoStep3);

        DeleteAllSpawnedCharacters();

        UnlockArea(AreaName.TOWN_HALL);
        ShowCutscene(_eatingFoodAloneScript, AreaName.PARK, new List<ID>() { giftRecipient }, 1.5f, () => _uiController.UnlockArea("Town Hall"));
    }

    //after buying a food item, unlock the town so you can give the food to the character
    private void DoStep2()
    {
        AdvanceStep();
        UnlockArea(AreaName.TOWN);
        _uiController.UnlockArea("Town");

        _giftMenu.OnGiveGift.AddListener(DoStep3);
    }

    //cutscene with 1 character. they say Hi and ask for food
    private async void DoStep1()
    {
        AdvanceStep();

        DeleteAllSpawnedCharacters();

        while (true) {
            if (Input.GetMouseButtonUp(0)) break;
            await Task.Yield();
        }

        UnlockArea(AreaName.GROCERY_STORE);

        var selected = CharacterManager.i.AllIDs()[0];
        ShowCutscene(_helloImHungryScript, AreaName.PARK, new List<ID>() { selected }, 1f, () => _uiController.UnlockArea("Grocery Store"));
    }

    //tutorial to say 'welcome to game, go make a character!'
    private void DoStep0()
    {
        AdvanceStep();

        //ShowTutorial(WelcomeTutorial)
    }

    private async void ShowCutscene(CutsceneScript script, AreaName Set, List<ID> ids, float delay = 0, System.Action callback = null)
    {
        _ = TownGameManager.i.ChangeArea(AreaName.NONE);
        
        var spawnedChars = _setController.ShowSet(Set, ids);
        if (spawnedChars.Count == 1) spawnedChars.Add(null);

        await Task.Delay(Mathf.RoundToInt(delay * 1000));

        CutsceneManager.i.StartCutscene(script.Script, _setController.Current.Camera, spawnedChars[0], spawnedChars[1], callback);
    }
}
