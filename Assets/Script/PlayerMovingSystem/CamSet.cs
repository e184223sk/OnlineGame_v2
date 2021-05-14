using UnityEngine;

public class CamSet : MonoBehaviour
{
    public Camera cam;
    void Awake() => _MainCamera.SetCamera(cam);
}
