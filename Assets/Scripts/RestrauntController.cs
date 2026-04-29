using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[System.Serializable]
public class SubgameSceneData
{
    [HideInInspector] public string DisplayName;
    public SubgameType Type;
    public int Index;
    [SerializeField] private GameObject _camera;
    [SerializeField] private List<GameObject> _toEnable;
    [SerializeField] private Transform _characterPos;
    [SerializeField] private CharacterAnimations _animation;

    public void OnValidate()
    {
        DisplayName = Type + " " + Index;
    }

    public void Hide(SpawnedCharacter character)
    {
        _camera.SetActive(false);
        foreach (var obj in _toEnable) obj.SetActive(false);
        if (character) character.AnimateFromEnum(_animation, false);
    }

    public void Show(SpawnedCharacter character)
    {
        _camera.SetActive(true);
        foreach (var obj in _toEnable) obj.SetActive(true);
        character.transform.position = _characterPos.position;
        character.transform.eulerAngles = _characterPos.eulerAngles;
        character.AnimateFromEnum(_animation);
        character.CharacterLookAt(_camera.transform, true);
    }
}


public class RestrauntController : MonoBehaviour
{
    [SerializeField] private Transform _characterSpawnPoint;

    [Header("Cameras")]
    [SerializeField] private GameObject startingCamera;
    [SerializeField] private GameObject stoveCamera;
    [SerializeField] private List<SubgameSceneData> _subgameScenes = new List<SubgameSceneData>();

    [Header("Effects")]
    [SerializeField] private CloudParticles characterSelectCloud;

    private SpawnedCharacter _spawnedCharacter;

    public SpawnedCharacter SpawnedCharacter => _spawnedCharacter;

    private void OnValidate()
    {
        foreach (var s in _subgameScenes) s.OnValidate();
    }

    public void ShowSubgameSceneCam(SubgameType type, int sceneIndex)
    {
        foreach (var s in _subgameScenes) s.Hide(_spawnedCharacter);

        bool found = false;
        foreach (var s in _subgameScenes) {
            if (s.Type == type && s.Index == sceneIndex) {
                s.Show(_spawnedCharacter);
                found = true;
                break;
            }
        }

        if (found) {
            startingCamera.SetActive(false);
            stoveCamera.SetActive(false);
        }
    }

    public SpawnedCharacter SpawnCharacter(ID id)
    {
        var character = CharacterManager.i.SpawnCharacter(id, _characterSpawnPoint);
        character.transform.SetParent(_characterSpawnPoint);
        character.transform.localPosition = Vector3.zero;
        return character;
    }

    //make camera controller 

    public void ResetCamera()
    {
        foreach (var s in _subgameScenes) s.Hide(_spawnedCharacter);
        startingCamera.SetActive(true);

        if (!_spawnedCharacter) return;
        _spawnedCharacter.transform.position = _characterSpawnPoint.position;
        _spawnedCharacter.transform.eulerAngles = _characterSpawnPoint.eulerAngles;
        _spawnedCharacter.CharacterLookAt(startingCamera.transform, true);
        _spawnedCharacter.AnimateFromEnum(CharacterAnimations.Standing);
    }

    public void DestroySpawnedCharacter()
    {
        if (_spawnedCharacter) Destroy(_spawnedCharacter.gameObject);
        _spawnedCharacter = null;
    }

    public void SpawnCharacterSelect(ID id)
    {
        //kill old character, spawn new

        DestroySpawnedCharacter();

        characterSelectCloud.SpawnCharacterSelectCloud();

        _spawnedCharacter = SpawnCharacter(id);

        _spawnedCharacter.GetComponent<SpawnedCharacter>().GrowCharacter(1.0f);

        _ = TriggerSpawnAnimation();
    }

    public async Task TriggerSpawnAnimation()
    {
        await Task.Delay(200);

        _spawnedCharacter.GetComponent<SpawnedCharacter>().TriggerFromString("Spawn");

        await Task.Delay(1000);

        _spawnedCharacter.GetComponent<SpawnedCharacter>().CharacterLookAt(startingCamera.transform);

    }



}
