﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class MoveJoint :MonobitEngine.MonoBehaviour
{

    public bool[] IKS;
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


    private void FixedUpdate()
    {
        IKS = new bool[4];
        IKS[0] = handL.enable;
        IKS[1] = handR.enable;
        IKS[2] = footL.enable;
        IKS[3] = footR.enable; 
    }

    void ReData()
    {
        IKS = new bool[4];
        handL.enable = IKS[0];
        handR.enable = IKS[1];
        footL.enable = IKS[2];
        footR.enable = IKS[3];
    } 


    //送信する情報をキューに追加
    public override void OnMonobitSerializeViewWrite(MonobitStream stream, MonobitMessageInfo info)
    {
        stream.Enqueue(Active);

        stream.Enqueue(lookAtObject.position);
        stream.Enqueue(lookAtObject.rotation);

        stream.Enqueue(waist.position);
        stream.Enqueue(waist.rotation);
        stream.Enqueue(handL.enable);
        stream.Enqueue(handR.enable);
        stream.Enqueue(footL.enable);
        stream.Enqueue(footR.enable);
        stream.Enqueue(handL.target.position);
        stream.Enqueue(handR.target.position);
        stream.Enqueue(footL.target.position);
        stream.Enqueue(footR.target.position);
        stream.Enqueue(handL.target.rotation);
        stream.Enqueue(handR.target.rotation);
        stream.Enqueue(footL.target.rotation);
        stream.Enqueue(footR.target.rotation);
    }

    public override void OnMonobitSerializeViewRead(MonobitStream stream, MonobitMessageInfo info)
    {
        Active = (bool)stream.Dequeue();

        lookAtObject.position =  (Vector3)stream.Dequeue();
        lookAtObject.rotation = (Quaternion)stream.Dequeue();

        waist.position = (Vector3)stream.Dequeue();
        waist.rotation = (Quaternion)stream.Dequeue();
        handL.enable = (bool)stream.Dequeue();
        handR.enable = (bool)stream.Dequeue();
        footL.enable = (bool)stream.Dequeue();
        footR.enable = (bool)stream.Dequeue();
        handL.target.rotation = (Quaternion)stream.Dequeue();
        handR.target.rotation = (Quaternion)stream.Dequeue();
        footL.target.rotation = (Quaternion)stream.Dequeue();
        footR.target.rotation = (Quaternion)stream.Dequeue();
        handL.target.position = (Vector3)stream.Dequeue();
        handR.target.position = (Vector3)stream.Dequeue();
        footL.target.position = (Vector3)stream.Dequeue();
        footR.target.position = (Vector3)stream.Dequeue();
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
        Weight = 1;
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

