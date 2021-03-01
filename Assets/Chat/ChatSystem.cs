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


    void Start()
    {
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
        chatDatas = new ChatData[uiData.Length];
    }
      
    void Update()
    {
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
    }

    public void New(ChatUSER user, string text)
    {
        flag = true;
        updateTimeColor = (user.COLOR + Color.white) / 2;
        for (int y = 0; y < 5; y++)
            chatDatas[y] = chatDatas[y + 1];
        chatDatas[5] = new ChatData() { color = user.COLOR, username = user.NAME, text = text };

        for (int y = 0; y < 6; y++)
        {
            if (uiData[y] != null && chatDatas[y] != null)
            {
                uiData[y].name_.text = chatDatas[y].username;
                uiData[y].text_.text = chatDatas[y].text;
                uiData[y].name_.color = uiData[y].text_.color = chatDatas[y].color;
            }
        }
    }
}
