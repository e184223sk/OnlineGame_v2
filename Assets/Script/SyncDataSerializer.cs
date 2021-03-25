using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 

public interface SyncDataSerializer
{
    string Serialize();
    void   DeSerialize();
}


public class test_xxx: SyncNetWorkBehavior, SyncDataSerializer
{
    public string Serialize() { return ""; }
    public void DeSerialize() { }

}