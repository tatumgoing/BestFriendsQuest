using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class CharacterSpawnLocation : MonoBehaviour
{
    [SerializeField] private CharacterAnimations animations;
    [SerializeField] private bool isMoving;

    [SerializeField] private SpawnedCharacter spawnedCharacter;

    public void Update()
    {
        if (!isMoving || spawnedCharacter == null) return;
        
        spawnedCharacter.transform.position = transform.position;
        spawnedCharacter.transform.localEulerAngles = transform.localEulerAngles - Vector3.up * 180;        
    }

    /// <summary>
    /// Configure a newly spawned characted (animations) based on this spot's information
    /// </summary>
    public async Task SetCharacter(SpawnedCharacter newChara)
    {
        spawnedCharacter = newChara;
        await Task.Delay(200);
        AnimateCharacter();       
    }


    /// <summary>
    /// Picks the right animation for the character based on this spot's information, and also rotates the character to face the right direction (since some animations are directional)
    /// </summary>
    void AnimateCharacter()
    {
        spawnedCharacter.gameObject.transform.localEulerAngles = new Vector3(gameObject.transform.localEulerAngles.x, gameObject.transform.localEulerAngles.y - 180, gameObject.transform.localEulerAngles.z);
        spawnedCharacter.AnimateFromEnum(animations);
    }
}
