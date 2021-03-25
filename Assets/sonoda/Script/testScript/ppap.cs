using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ppap : SyncNetWorkBehavior
{

    private int _myID;

    private MonobitEngine.MonobitView _monobitView;

    [SerializeField]
    public string _greeting;

    private GameObject obj;




    // Start is called before the first frame update
    void Start()
    {

        _monobitView = GetComponent<MonobitEngine.MonobitView>();


        _myID = MonobitEngine.MonobitNetwork.playerList.Length;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("自分のID？" + _myID);
        /*if (Input.GetKeyDown(KeyCode.O))
        {
            MonobitEngine.MonobitNetwork.Instantiate("Cube", new Vector3(0, 7.6f, 1), Quaternion.identity, 0);
        }*/
        if (MonobitEngine.MonobitNetwork.inRoom)
        {
            if(obj == null)
            {
                obj = MonobitEngine.MonobitNetwork.Instantiate("Cube", new Vector3(Random.Range(-5, 5), 0, 1f), Quaternion.identity, 0);
                
            }

            if (Input.GetKey(KeyCode.W)) obj.transform.position += Vector3.up * Time.deltaTime;
            if (Input.GetKey(KeyCode.S)) obj.transform.position -= Vector3.up * Time.deltaTime;
            if (Input.GetKey(KeyCode.A)) obj.transform.position += Vector3.left * Time.deltaTime;
            if (Input.GetKey(KeyCode.D)) obj.transform.position -= Vector3.left * Time.deltaTime;
        }

    }
    string name = "";

    private void OnGUI()
    {

        GUILayout.Label(obj.name);
        name = GUILayout.TextField(name);
        if (GUILayout.Button("Enter"))
        {
            obj.name = name;
        }
    }
}
