using UnityEngine;

public class Singou_EditorSys : MonoBehaviour
{ 
    public float[] rotations;
    void Start() => Destroy(this);

    public void OnValidate()
    {
        if (rotations.Length == 0)
        {
            rotations = new float[4];
            return;
        }

        for (int v = 0; v < rotations.Length; v++)
        { 
            transform.Find("High/" + v).localRotation =
            transform.Find("Mid/" + v).localRotation =
            transform.Find("Low/" + v).localRotation =
                            Quaternion.Euler(0, rotations[v], 0);
        }
    }
}
