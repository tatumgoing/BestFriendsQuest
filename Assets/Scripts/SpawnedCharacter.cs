using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public enum CharacterAnimations { Grilling, Spawn, Standing, Sitting, SittingGround, Walking };

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

    //Growing
    private float growTimer;
    private float growRate;

    [SerializeField] private AnimationCurve growCurve;


    private void Update()
    {


        if (Time.time < growTimer)
        {
            var progress = (growTimer - Time.time / growRate);

            progress = growCurve.Evaluate(progress);

            transform.localScale = Vector3.Lerp(new Vector3(1,1,1),new Vector3(0, 0, 0), progress );
        }
    }

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
        animator.SetBool(anim.ToString(), true);

    }

    public void AnimateFromString(String anim)
    {
        animator.SetBool(anim, true);

    }

    public void TriggerFromString(String anim)
    {
        animator.SetTrigger(anim.ToString());
        Debug.Log(anim);
    }


    public void GrowCharacter(float growTime)
    {
        growRate = growTime;

        growTimer = Time.time + growTime;
        transform.localScale = new Vector3(0, 0, 0);

    }

}
