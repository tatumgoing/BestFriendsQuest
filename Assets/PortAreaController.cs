using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortAreaController : MonoBehaviour
{
    [SerializeField] private float _resultsWaitTime = 2;
    [SerializeField] private QuestUIController _uiController;
    [SerializeField] private CharacterSpawnLocation _char1Spawn;
    [SerializeField] private CharacterSpawnLocation _char2Spawn;
    [SerializeField] private Transform _chestStartPos;
    [SerializeField] private Transform _chestEndPos;
    [SerializeField] private Transform _chest;
    [SerializeField] private ScrollingChildren _debris;
    [SerializeField] private GameObject _camera;

    private RuntimeQuestData _questData;
    private SpawnedCharacter _char1 = null;
    private SpawnedCharacter _char2 = null;
    private bool _readyToShowResults = false;

    public Transform Camera => _camera.transform;

    private void OnEnable()
    {
        _debris.enabled = false;
    }

    public List<SpawnedCharacter> ShowCharacters(RuntimeQuestData questData)
    {
        _questData = questData;
        _debris.enabled = true;
        _chest.position = _chestStartPos.position;

        _char1 = CharacterManager.i.SpawnCharacter(questData.Character1, _char1Spawn.transform);
        _ = _char1Spawn.SetCharacter(_char1);

        _char2 = CharacterManager.i.SpawnCharacter(questData.Character2, _char2Spawn.transform);
        _ = _char2Spawn.SetCharacter(_char2);

        _readyToShowResults = false;

        return new List<SpawnedCharacter>() { _char1, _char2 };
    }

    [ButtonMethod]
    public void FinishWalking()
    {
        _readyToShowResults = true;
    }

    private void Update()
    {
        if (!_readyToShowResults) _chest.position = _chestStartPos.position;

        if (!_debris.enabled) return;

        if (Vector3.Distance(_chestEndPos.position, _chestStartPos.position) < Vector3.Distance(_chest.position, _chestStartPos.position)) {
            _debris.enabled = false;
            _char1.AnimateFromEnum(CharacterAnimations.Walking, false);
            _char2.AnimateFromEnum(CharacterAnimations.Walking, false);

            WaitThenShowResults();
        }
    }

    private async void WaitThenShowResults()
    {
        await System.Threading.Tasks.Task.Delay(Mathf.RoundToInt(_resultsWaitTime * 1000));

        if (_char1 != null) Destroy(_char1.gameObject);
        if (_char2 != null) Destroy(_char2.gameObject);
        _char1 = null;
        _char2 = null;

        _uiController.ShowResults();
    }
}
