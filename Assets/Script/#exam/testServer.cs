using UnityEngine;

public class testServer : MonoBehaviour
{
    void Start()
    {
        NetData.server = new Server(NetData.ServerAddress, NetData.ServerPASS, NetData.ServerDomain);
        NetData.server.MakeDirectory("dkso_ddo", FIN , Debug.LogError);
    }
    void FIN() => Debug.Log("CLEAR"); 
}
