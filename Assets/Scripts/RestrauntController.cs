using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class RestrauntController : MonoBehaviour
{
    [SerializeField] private Transform _characterSpawnPoint;

    [Header("Cameras")]

    [SerializeField] private GameObject startingCamera;
    [SerializeField] private GameObject stoveCamera;

    [Header("Effects")]

    [SerializeField] private CloudParticles characterSelectCloud;

    public GameObject characterSelectSpawnedCharacter;

    public GameObject SpawnCharacter(ID id)
    {
        
        var character = CharacterManager.i.SpawnCharacter(id, _characterSpawnPoint);
        character.transform.SetParent(_characterSpawnPoint);
        character.transform.localPosition = Vector3.zero;
        return character.gameObject;
    }

    //make camera controller 

    public void ResetCamera()
    {
        startingCamera.SetActive(true);
    }

    // set next camera

    public void NextCamera()
    {

    }

    public void DestroySpawnedCharacter()
    {
        if (characterSelectSpawnedCharacter) Destroy(characterSelectSpawnedCharacter);
    }

    public void SpawnCharacterSelect(ID id)
    {
        //kill old character, spawn new

        DestroySpawnedCharacter();

        characterSelectCloud.SpawnCharacterSelectCloud();

        characterSelectSpawnedCharacter = SpawnCharacter(id);

        characterSelectSpawnedCharacter.GetComponent<SpawnedCharacter>().GrowCharacter(1.0f);

        TriggerSpawnAnimation();
    }

    public async Task TriggerSpawnAnimation()
    {
        await Task.Delay(200);

        characterSelectSpawnedCharacter.GetComponent<SpawnedCharacter>().TriggerFromString("Spawn");

        await Task.Delay(1000);

        characterSelectSpawnedCharacter.GetComponent<SpawnedCharacter>().CharacterLookAt(startingCamera.transform);

    }



}
