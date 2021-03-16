using UnityEngine;

public class TestServer_2 : MonoBehaviour
{
    public bool read_data, read_log;

    void Update()
    {
        if (read_data)
        {
            Debug.Log(NetData.server.GetText(NetData.Get__DATA_TXT));
            read_data = false;
        }
        if (read_log)
        {
            Debug.Log(NetData.server.GetText(NetData.Get__LOG_TXT));
            read_log = false;
        } 
    }
}