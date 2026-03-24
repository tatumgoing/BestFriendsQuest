using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class RestrauntController : MonoBehaviour
{
    [SerializeField] private Transform _characterSpawnPoint;

    [Header("Cameras")]

    [SerializeField] private GameObject startingCamera;
    [SerializeField] private GameObject stoveCamera;

    [Header("Effects")]

    [SerializeField] private GameObject characterSelectCloud;

    private GameObject characterSelectSpawnedCharacter;

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

    public void SpawnCharacterSelect(ID id)
    {
        Debug.Log("Changing characters!");

        //kill old character, spawn new

        if (characterSelectSpawnedCharacter != null)
        {
            Destroy(characterSelectSpawnedCharacter);
        }

        SpawnCharacterSelectCloud(id);
    }

    public async Task SpawnCharacterSelectCloud(ID id)
    {
        characterSelectCloud.SetActive(true);

        await Task.Delay(500);
        characterSelectSpawnedCharacter = SpawnCharacter(id);

        await Task.Delay(500);
        characterSelectCloud.SetActive(false);
    }
   
}
