using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestUIController : MonoBehaviour
{
    [SerializeField] private QuestMapController _map;
    [SerializeField] private InProgressQuestMenu _inProgress;
    [SerializeField] private PortAreaController _areaController;
    [SerializeField] private BFQResultsScreen _results;
    [SerializeField] private GameObject _wipe;
    [SerializeField] private float _cutsceneWaitDelay = 2;
    [SerializeField] private QuestMapSelector _mapSelector;

    [Header("Cutscenes")]
    [SerializeField] private List<TextAsset> _scripts;
    [SerializeField] private TextAsset _demoScript;

    private RuntimeQuestData _currentQuest = null;

    private void OnEnable()
    {
        var loadedRegion = SaveSystem.LoadRegion();
        if (loadedRegion == "") {
            _mapSelector.gameObject.SetActive(true);
        }
        else {
            _mapSelector.gameObject.SetActive(false);

            var selectedMap = SaveSystem.GetSelectedMap();
            SelectMap(selectedMap);

            if (_currentQuest != null) _inProgress.Show(_currentQuest);
        }

        _results.gameObject.SetActive(false);
        _map.gameObject.SetActive(_currentQuest == null);
    }

    public void SelectMap(MapData selectedMap)
    {
        var islands = GetComponentsInChildren<QuestIsland>(true);
        for (int i = 0; i < selectedMap.Quests.Count; i++) {
            islands[i].Initialize(selectedMap.Quests[i]);
        }

        _currentQuest = SaveSystem.LoadBFQuest(selectedMap.Quests);
    }

    [ButtonMethod]
    public void ResetCompletedQuests()
    {
        SaveSystem.ResetCompletedBFQuests();
    }

    public async void StartWalkingAnimation()
    {
        _wipe.SetActive(true);
        await Task.Delay(Mathf.RoundToInt(0.5f * 1000));

        _inProgress.gameObject.SetActive(false);
        var spawnedCharacters = _areaController.ShowCharacters(_currentQuest);

        await Task.Delay (Mathf.RoundToInt(_cutsceneWaitDelay * 1000));

        var chosenCutscene = _scripts[Random.Range(0, _scripts.Count)];
        if (TownGameManager.i.DemoMode && DemoController.i.Step == 11) chosenCutscene = _demoScript;
        CutsceneManager.i.StartCutscene(chosenCutscene, _areaController.Camera, spawnedCharacters[0], spawnedCharacters[1], ShowChest);
    }

    private void ShowChest()
    {
        _areaController.FinishWalking();
    }

    public void StartQuest(Quest questData, ID speaker1, ID speaker2)
    {
        _map.gameObject.SetActive(false);
        _currentQuest = new RuntimeQuestData(questData, speaker1, speaker2);
        _inProgress.Show(_currentQuest);

        SaveSystem.SaveBFQuest(_currentQuest);
    }

    public void ShowResults()
    {
        if (!_areaController) return;
        
        _areaController.gameObject.SetActive(false);
        _results.ShowResults(_currentQuest);

        CharacterManager.i.IncreaseHappiness(_currentQuest.Character1, -100);
        CharacterManager.i.IncreaseHappiness(_currentQuest.Character2, -100);

        SaveSystem.DeleteSavedQuest(_currentQuest.QuestData);
    }

    public void ResetQuest()
    {
        print("Resetting quest");
        _areaController.gameObject.SetActive(true);
        _currentQuest = null; 
        _map.gameObject.SetActive(true);
    }
}
