using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test_chat : MonoBehaviour
{ 
    public ChatData data = new ChatData();
    public Text text;
    public string username, text_;
    public Color color;
     
    // Update is called once per frame
    void Update()
    { 
        data.color = color;
        data.username = username;
        data.text = text_;

        text.color = data.color;
        text.text = data.GetDrawText();
    }
}
