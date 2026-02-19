using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CharacterAnimations { Standing, Sitting, Walking };
public class CharacterSpawnLocation : MonoBehaviour
{
    
    [SerializeField] CharacterAnimations animations;
    [SerializeField] bool isMoving;

    [SerializeField] SpawnedCharacter spawnedCharacter;

    public void Update()
    {
        if (isMoving)
        {
            if (spawnedCharacter != null)
            {
                spawnedCharacter.gameObject.transform.position = gameObject.transform.position;
                //spawnedCharacter.gameObject.transform.localRotation = gameObject.transform.localRotation;
                spawnedCharacter.gameObject.transform.localEulerAngles = new Vector3(gameObject.transform.localEulerAngles.x, gameObject.transform.localEulerAngles.y - 180, gameObject.transform.localEulerAngles.z);
            }

        }
    }
    public void SetCharacter(SpawnedCharacter newChara)
    {
        // assign and place character
        spawnedCharacter = newChara;
        AnimateCharacter();

       
    }


    void AnimateCharacter()
    {
        spawnedCharacter.gameObject.transform.localEulerAngles = new Vector3(gameObject.transform.localEulerAngles.x, gameObject.transform.localEulerAngles.y - 180, gameObject.transform.localEulerAngles.z);
        spawnedCharacter.AnimateFromEnum(animations);
    }


}
