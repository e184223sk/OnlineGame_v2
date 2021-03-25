using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;


public class test4 : MonoBehaviour
{
    public void Start()
    {
        var target = GameObject.Find("i").GetComponent<test4_subs>();
        var me = GetComponent<test4_subs>();

        string t = SerializeText.Serialize<test4_subs, test4_subs_dataset>(me);

        SerializeText.DeSerialize<test4_subs, test4_subs_dataset>(ref target, t); 
    }
}















public interface DataSetinterface<T,M> where M : new()
{
    void DES(ref T ta);
    void SRL(T d); 
}


public class SerializeText 
{ 
    public static string Serialize<T,M>(T monoCLASS) where M : DataSetinterface<T, M>, new()
    {
        M m = new M();
        m.SRL(monoCLASS);
        return JsonUtility.ToJson(m); 
    }

    public static void DeSerialize<T,M>(ref T target, string data) where M : DataSetinterface<T, M>, new()
    {
        JsonUtility.FromJson<M>(data).DES(ref target) ;  
    }

}


/*
 public class SerializeText 
{


    public static string Serialize<T>(T data) where T : SyncNetWorkBehavior
    { 
        return JsonUtility.ToJson(data);
        XmlSerializer serializer = new XmlSerializer(typeof(T)); 
        MemoryStream ms = new MemoryStream();

        serializer.Serialize(ms, data);
         
        ms.Position = 0;
        StreamReader sr = new StreamReader(ms);
        string xx = (sr.Peek() > -1) ? sr.ReadToEnd() : "";
        sr.Close();
        ms.Close(); 
        return xx;  
    }

    public static T DeSerialize<T>(string data) where T : SyncNetWorkBehavior
    {
        return JsonUtility.FromJson<T>(data);
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        MemoryStream ms = new MemoryStream();
        //var tt = bn MemoryStream();
        //return (T)serializer.Deserialize(tt); 
    }

}
     
     */
