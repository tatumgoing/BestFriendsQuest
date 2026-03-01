using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class SpawnedCharacter : MonoBehaviour
{
    [SerializeField] private CharacterMetaController _characterController;
    [SerializeField] public ID ID;

    [SerializeField] private Animator animator;

    public void LoadFromString(string saveString)
    {
        _characterController.LoadFromString(saveString);
        this.ID = _characterController.Data.ID;

        gameObject.name = _characterController.Data.Name + " (spawned character)";
    }
    
    public void AnimateFromEnum(CharacterAnimations anim)
    {
        if (animator != null)
        {

            if (anim == CharacterAnimations.Grilling)
            {
                Grilling();
            }
            if (anim == CharacterAnimations.Sitting)
            {
                Sitting();
            }
            if (anim == CharacterAnimations.SittingGround)
            {
                SittingGround();
            }
            else if (anim == CharacterAnimations.Standing)
            {
                Standing();
            }
            else if(anim == CharacterAnimations.Walking)
            {
                Walking();
            }

        }
    }

    public void Grilling()
    {
        animator.SetBool("Grilling", true);
    }
    public void Sitting()
    {
        animator.SetBool("Sitting", true);
    }
    public void SittingGround()
    {
        animator.SetBool("SittingGround", true);
    }
    public void Standing()
    {
        animator.SetBool("Standing", true);
    }
    public void Walking()
    {
        animator.SetBool("Walking", true);
    }
}
