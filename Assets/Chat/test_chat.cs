using UnityEngine;

public class test_chat : MonoBehaviour
{
    public string user;
    public Color color;

    void Update() => ChatSystem.userDatas = new ChatUSER(user, color);
}