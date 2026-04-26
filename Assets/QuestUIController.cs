using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUIController : MonoBehaviour
{
    [SerializeField] private QuestMapController _map;

    private RuntimeQuestData _currentQuest = null;

    public void StartQuest(Quest questData, ID id1, ID id2)
    {
        _currentQuest = new RuntimeQuestData(questData, id1, id2);
    }

    private void Update()
    {
        if (_currentQuest != null) _map.UpdateIslands(_currentQuest);
    }
}
