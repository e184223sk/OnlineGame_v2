using UnityEngine;

public class UnderSeaBubble_SetArray : MonoBehaviour
{ 
    void Start()
    {
        Wave.bubbleList.Add(transform);
        Destroy(this);
    } 
}
