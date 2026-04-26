using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUIController : MonoBehaviour
{
    [SerializeField] private QuestMapController _map;
    [SerializeField] private InProgressQuestMenu _inProgress;
    [SerializeField] private PortAreaController _areaController;
    [SerializeField] private BFQResultsScreen _results;

    private RuntimeQuestData _currentQuest = null;

    private void OnEnable()
    {
        _map.gameObject.SetActive(_currentQuest == null);
    }

    public void StartWalkingAnimation()
    {
        _areaController.ShowCharacters(_currentQuest);
    }

    public void StartQuest(Quest questData, ID id1, ID id2)
    {
        _map.gameObject.SetActive(false);
        _currentQuest = new RuntimeQuestData(questData, id1, id2);
        _inProgress.Show(_currentQuest);
    }

    public void ShowResults()
    {
        _areaController.gameObject.SetActive(false);
        _results.ShowResults(_currentQuest);
    }

    public void ResetQuest()
    {
        _areaController.gameObject.SetActive(true);
        _currentQuest = null; 
        _map.gameObject.SetActive(true);
    }
}
