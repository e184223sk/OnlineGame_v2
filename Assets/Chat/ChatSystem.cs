using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatSystem : MonoBehaviour
{
    public static ChatSystem system;
    ChatData[] chatDatas;
    ChatTextUI[] uiData;
    bool flag;
    float tc; 
    Image space; 
    Color updateTimeColor;
    public static ChatUSER userDatas;
    InputField textBox;
    Text inputErrorText;

    float errorAlpha;
    void Start()
    {
        inputErrorText = transform.Find("inputError").GetComponent<Text>();
        inputErrorText.enabled = false;
        textBox = transform.Find("InputField").GetComponent<InputField>();
        textBox.onEndEdit.AddListener(PushChat);
        textBox.onValueChanged.AddListener(WritingChat);
        system = this;
        
        space = transform.Find("space/colorEffect").GetComponent<Image>();
        uiData = new ChatTextUI[6]
        {
            new ChatTextUI(transform.Find("space/0").transform),
            new ChatTextUI(transform.Find("space/1").transform),
            new ChatTextUI(transform.Find("space/2").transform),
            new ChatTextUI(transform.Find("space/3").transform),
            new ChatTextUI(transform.Find("space/4").transform),
            new ChatTextUI(transform.Find("space/5").transform),
        };
        chatDatas = new ChatData[6]
        {
            new ChatData(),
            new ChatData(),
            new ChatData(),
            new ChatData(),
            new ChatData(),
            new ChatData()
        };
    }
      
    void Update()
    {
        if (Key.FL.Down)
        {
            textBox.Select();
        }
        if (flag)
        {
            tc += Time.deltaTime*1.3f;
            space.color = new Color(updateTimeColor.r, updateTimeColor.g, updateTimeColor.b, Mathf.Sin(tc)/3);

            if (tc > 1)
            {
                tc = 0;
                flag = false;
                space.color = new Color();
            }
        }

        errorAlpha += Time.deltaTime;
        inputErrorText.color = new Color
        (
            inputErrorText.color.r,
            inputErrorText.color.g,
            inputErrorText.color.b,
            Mathf.Sin(errorAlpha * 0.7f + 0.3f)
        );
    }

    public void New(ChatUSER user, string text)
    {
        Debug.Log("call New Method!!" + "::: userN:" + user.NAME + "/// text:" + text + "/// color: " + user.COLOR.r + ":" + user.COLOR.g + ":" + user.COLOR.b + ":" + user.COLOR.a);
        flag = true;
        updateTimeColor = (user.COLOR + Color.white) / 2;
        for (int y = 0; y < 5; y++)
            chatDatas[y] = chatDatas[y + 1];
        chatDatas[5] = new ChatData() { color = user.COLOR, username = user.NAME, text = text };

        for (int y = 0; y < 6; y++)
        {
            uiData[y].name_.text = chatDatas[y].username;
            uiData[y].text_.text = chatDatas[y].text;
            uiData[y].name_.color = uiData[y].text_.color = chatDatas[y].color;
        }
    }



    private void OnDestroy()
    {
        userDatas = null;
        system = null;
    }
    
    public void PushChat(string e)
    { 
        if (Input.GetKey(KeyCode.Return))
        {
            string tx = textBox.text;
            foreach (var t in ChatSystem_DataBase.forbiddens)
                tx = tx.Replace(tx, "".PadLeft(tx.Length, '*'));

            ChatSync.SEND(ChatSystem.userDatas, textBox.text);
            textBox.text = "";
            Debug.Log("called2");
        }
    }



    public void WritingChat(string x)
    {
        inputErrorText.enabled = textBox.text.Length >= 32;
    }
}
