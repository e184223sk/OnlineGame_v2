using UnityEngine;
using UnityEngine.UI;

public class ChatTextUI
{
    public Text name_, text_;
    public ChatTextUI(Transform root_)
    {
        name_ = root_.Find("name").GetComponent<Text>();
        text_ = root_.Find("text").GetComponent<Text>();
    } 
}