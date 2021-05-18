using System;
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

        stream.Enqueue(lookAtObject.localPosition);
        stream.Enqueue(lookAtObject.localRotation);

        stream.Enqueue(waist.localPosition);
        stream.Enqueue(waist.localRotation);
        stream.Enqueue(handL.enable);
        stream.Enqueue(handR.enable);
        stream.Enqueue(footL.enable);
        stream.Enqueue(footR.enable);
        stream.Enqueue(handL.target.localRotation);
        stream.Enqueue(handR.target.localRotation);
        stream.Enqueue(footL.target.localRotation);
        stream.Enqueue(footR.target.localRotation);
        stream.Enqueue(handL.target.localPosition);
        stream.Enqueue(handR.target.localPosition);
        stream.Enqueue(footL.target.localPosition);
        stream.Enqueue(footR.target.localPosition);
    }

    public override void OnMonobitSerializeViewRead(MonobitStream stream, MonobitMessageInfo info)
    {
        Active = (bool)stream.Dequeue();

        lookAtObject.localPosition =  (Vector3)stream.Dequeue();
        lookAtObject.localRotation = (Quaternion)stream.Dequeue();

        waist.localPosition = (Vector3)stream.Dequeue();
        waist.localRotation = (Quaternion)stream.Dequeue();
        handL.enable = (bool)stream.Dequeue();
        handR.enable = (bool)stream.Dequeue();
        footL.enable = (bool)stream.Dequeue();
        footR.enable = (bool)stream.Dequeue();
        handL.target.localRotation = (Quaternion)stream.Dequeue();
        handR.target.localRotation = (Quaternion)stream.Dequeue();
        footL.target.localRotation = (Quaternion)stream.Dequeue();
        footR.target.localRotation = (Quaternion)stream.Dequeue();
        handL.target.localPosition = (Vector3)stream.Dequeue();
        handR.target.localPosition = (Vector3)stream.Dequeue();
        footL.target.localPosition = (Vector3)stream.Dequeue();
        footR.target.localPosition = (Vector3)stream.Dequeue();
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

