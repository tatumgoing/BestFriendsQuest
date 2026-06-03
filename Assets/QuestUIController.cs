using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class QuestUIController : MonoBehaviour
{
    [SerializeField] private QuestMapController _map;
    [SerializeField] private InProgressQuestMenu _inProgress;
    [SerializeField] private PortAreaController _areaController;
    [SerializeField] private BFQResultsScreen _results;
    [SerializeField] private GameObject _wipe;
    [SerializeField] private float _cutsceneWaitDelay = 2;

    [Header("Cutscenes")]
    [SerializeField] private List<TextAsset> _scripts;

    private RuntimeQuestData _currentQuest = null;
    private ID _id1;
    private ID _id2;

    private void OnEnable()
    {
        _results.gameObject.SetActive(false);
        _map.gameObject.SetActive(_currentQuest == null);
    }

    public async void StartWalkingAnimation()
    {
        _wipe.SetActive(true);
        await Task.Delay(Mathf.RoundToInt(0.5f * 1000));

        _inProgress.gameObject.SetActive(false);
        _areaController.ShowCharacters(_currentQuest);

        await Task.Delay (Mathf.RoundToInt(_cutsceneWaitDelay * 1000));

        var chosenCutscene = _scripts[Random.Range(0, _scripts.Count)];
        CutsceneManager.i.StartCutscene(chosenCutscene, _id1, _id2, ShowChest);
    }

    private void ShowChest()
    {
        _areaController.FinishWalking();
    }

    public void StartQuest(Quest questData, ID id1, ID id2)
    {
        _id1 = id1;
        _id2 = id2;

        _map.gameObject.SetActive(false);
        _currentQuest = new RuntimeQuestData(questData, id1, id2);
        _inProgress.Show(_currentQuest);
    }

    public void ShowResults()
    {
        _areaController.gameObject.SetActive(false);
        _results.ShowResults(_currentQuest);

        CharacterManager.i.IncreaseHappiness(_currentQuest.Character1, -100);
        CharacterManager.i.IncreaseHappiness(_currentQuest.Character2, -100);
    }

    public void ResetQuest()
    {
        _areaController.gameObject.SetActive(true);
        _currentQuest = null; 
        _map.gameObject.SetActive(true);
    }
}
