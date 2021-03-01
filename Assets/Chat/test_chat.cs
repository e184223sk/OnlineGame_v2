using UnityEngine;

public class test_chat : MonoBehaviour
{
    public ChatUSER user; 

    void Update() => ChatSystem.userDatas = user;
}