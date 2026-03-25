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

    [Header("Head Look At")]

    [SerializeField] private Transform head;
    [SerializeField] private Transform headForward;
    [SerializeField] private float maxAngle, minAngle;

    private Transform lookAtTarget;

    [SerializeField] private float lookSpeed;
    private bool isLooking;
    private Quaternion lastRotation;


    private void LateUpdate()
    {
        UpdateLookAt();
    }
    public void LoadFromString(string saveString)
    {
        _characterController.LoadFromString(saveString);
        this.ID = _characterController.Data.ID;

        gameObject.name = _characterController.Data.Name + " (spawned character)";
    }

    public void CharacterLookAt(Transform target)
    {
        lookAtTarget = target;
    }

    public void UpdateLookAt()
    {
        if (lookAtTarget)
        {
            //head.LookAt(lookAtTarget);

            //head.right = lookAtTarget.position - head.position;

            Vector3 Direction = (lookAtTarget.position - head.position).normalized;
            float angle = Vector3.SignedAngle(Direction, headForward.position, headForward.up);

            if (angle < maxAngle && angle > minAngle)
            {

                if (!isLooking)
                {
                    isLooking = true;
                    lastRotation = head.rotation;
                }

                Quaternion targetRotation = Quaternion.LookRotation(lookAtTarget.position - head.position);
                lastRotation = Quaternion.Slerp(lastRotation, targetRotation, lookSpeed * Time.deltaTime);

                head.rotation = lastRotation;
            }
        }
        else
        {
            lastRotation = Quaternion.Slerp(lastRotation, headForward.rotation, lookSpeed * Time.deltaTime);
            head.rotation = lastRotation;   
        }
    }

    public void EndCharacterLookAt()
    {
        lookAtTarget = null;
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
