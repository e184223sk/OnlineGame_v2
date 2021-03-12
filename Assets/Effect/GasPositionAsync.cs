using UnityEngine;

public class GasPositionAsync : MonoBehaviour
{
    [SerializeField]
    Transform target;

    void Start() { if (target == null) Destroy(this); }
    void Update() { if (target != null) transform.position = target.position; }

    #if UNITY_EDITOR
    void OnDrawGizmos() => Update();
    #endif
}
