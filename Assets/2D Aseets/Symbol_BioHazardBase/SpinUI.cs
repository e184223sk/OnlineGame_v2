using UnityEngine;

public class SpinUI : MonoBehaviour
{
    public float Speed;
    RectTransform rect;
    void Start() => rect = GetComponent<RectTransform>();
    void Update() => rect.Rotate(Vector3.forward * Speed * Time.deltaTime);
}
