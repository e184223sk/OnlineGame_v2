﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveJoint : MonoBehaviour
{
    public bool Active;
    public Transform lookAtObject = null;
    public Transform waist = null;
    public IKDATA handR = null;
    public IKDATA handL = null;
    public IKDATA footR = null;
    public IKDATA footL = null;
    
    public Animator animator;

    private void Start()
    {
        handR.INIT(AvatarIKGoal.RightHand);
        handL.INIT(AvatarIKGoal.LeftHand);
        footR.INIT(AvatarIKGoal.RightFoot);
        footL.INIT(AvatarIKGoal.LeftFoot);
    }

    void OnAnimatorIK(int t)
    {
        if (lookAtObject != null)
        {
            animator.SetLookAtWeight(1.0f, 0.8f, 1.0f, 0.0f, 0f);
            animator.SetLookAtPosition(lookAtObject.position);
        }

        if (waist != null)
        {
            animator.bodyPosition = waist.position;
            animator.bodyRotation = waist.rotation;
        }

        handL.Update(animator);
        handR.Update(animator);
        footL.Update(animator);
        footR.Update(animator);
    } 
}



[System.Serializable]
public class IKDATA
{
    public bool enable;
    public Transform target;


    [Range(0f, 1f), SerializeField]
    public float weight;

    public float Weight
    {
        get => weight;
        set => weight = value > 1 ? 1 : (value < 0 ? 0 : value);
    }

    private AvatarIKGoal goal;

    public void INIT(AvatarIKGoal goal)
    {
        this.goal = goal;
    }


    public void Update(Animator animator)
    {
        if (enable)
        {
            if (target == null) return;
            animator?.SetIKPositionWeight(goal, weight);
            animator?.SetIKRotationWeight(goal, weight);
            animator?.SetIKPosition(goal, target.position);
            animator?.SetIKRotation(goal, target.rotation);
        }
        else
        {
            Weight -= Time.deltaTime;
        }
        
    }
}
