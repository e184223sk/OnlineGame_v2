using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class test4_subs  : SyncNetWorkBehavior 
{ 
    public int i1; 
    public float f1; 
    public Vector3 v1;
   
}


public class test4_subs_dataset : DataSetinterface<test4_subs, test4_subs_dataset>
{ 
    public int i1;
    public float f1;
    public Vector3 v1;

     

    public void DES(ref test4_subs ta)
    { 
        ta.i1 = i1;
        ta.v1 = v1;
        ta.f1 = f1;
    }



    public void SRL(test4_subs d)
    {
        i1 = d.i1;
        v1 = d.v1;
        f1 = d.f1;
    }
     
}
