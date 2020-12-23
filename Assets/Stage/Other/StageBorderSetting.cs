using UnityEngine;

public class StageBorderSetting : MonoBehaviour
{
    [SerializeField]
    Vector3_StageBorder Border; 
    static Vector3_StageBorder _border;
    
    void Start() { Destroy(this); }


    /// <summary>
    /// ステージの境界線をVector3_StageBorder型で返します
    /// <para>Vector3_StageBorder型は</para>
    /// <para>インスペクタ上に表示されているものと同じ型です。</para>
    /// <para>そのため同じ変数名で保持しています。</para>
    /// </summary>
    public static Vector3_StageBorder GetBorder => _border; 


    /// <summary>
    /// ステージの境界線をStageBorderParameter型で返します
    /// <para>StageBorderParameter型は</para>
    /// <para>正と負の極点を保持しています</para>
    /// </summary>
    public static StageBorderParameter GetStageBorder => _border;












#if UNITY_EDITOR

    void OnValidate()
    {
        _border = Border;
        var d = (Border.size + Vector3.one * Border.thickness) / 2; 
        SetCollider("+X", d.x * new Vector3(01, 0, 0), Border.thickness * new Vector3(1, 2, 2) + new Vector3(0, Border.size.y, Border.size.z));
        SetCollider("-X", d.x * new Vector3(-1, 0, 0), Border.thickness * new Vector3(1, 2, 2) + new Vector3(0, Border.size.y, Border.size.z));
        SetCollider("+Y", d.y * new Vector3(0, 01, 0), Border.thickness * new Vector3(2, 1, 2) + new Vector3(Border.size.x, 0, Border.size.z));
        SetCollider("-Y", d.y * new Vector3(0, -1, 0), Border.thickness * new Vector3(2, 1, 2) + new Vector3(Border.size.x, 0, Border.size.z));
        SetCollider("+Z", d.z * new Vector3(0, 0, 01), Border.thickness * new Vector3(2, 2, 1) + new Vector3(Border.size.x, Border.size.y, 0));
        SetCollider("-Z", d.z * new Vector3(0, 0, -1), Border.thickness * new Vector3(2, 2, 1) + new Vector3(Border.size.x, Border.size.y, 0));
    }



    void SetCollider(string _name, Vector3 pos, Vector3 size)
    {
        var d = transform.Find(_name);
        d.position = pos+ Border.basePosition;
        d.GetComponent<BoxCollider>().size = size;
    }



    private void OnDrawGizmos()
    {
        var s = Border.basePosition - Border.size / 2; 
        var e = Border.basePosition + Border.size / 2;
        DrawLINE(s, new Vector3(e.x, s.y, s.z), Color.red  );
        DrawLINE(s, new Vector3(s.x, e.y, s.z), Color.green);
        DrawLINE(s, new Vector3(s.x, s.y, e.z), Color.blue );
        DrawLINE(new Vector3(e.x, e.y, s.z), new Vector3(e.x, e.y, e.z), Color.white);
        DrawLINE(new Vector3(e.x, s.y, e.z), new Vector3(e.x, e.y, e.z), Color.white);
        DrawLINE(new Vector3(s.x, e.y, e.z), new Vector3(e.x, e.y, e.z), Color.white);

        DrawLINE(new Vector3(s.x, e.y, e.z), new Vector3(s.x, s.y, e.z), Color.white);
        DrawLINE(new Vector3(s.x, e.y, e.z), new Vector3(s.x, e.y, s.z), Color.white);

        DrawLINE(new Vector3(e.x, s.y, e.z), new Vector3(s.x, s.y, e.z), Color.white);
        DrawLINE(new Vector3(e.x, s.y, e.z), new Vector3(e.x, s.y, s.z), Color.white);
        DrawLINE(new Vector3(e.x, e.y, s.z), new Vector3(s.x, e.y, s.z), Color.white);
        DrawLINE(new Vector3(e.x, e.y, s.z), new Vector3(e.x, s.y, s.z), Color.white); 
    }



    void DrawLINE(Vector3 s, Vector3 e, Color c)
    {
        int m = 81;
        m += 1 - m % 2;
        for (int g = 0; g < m; g++)
        { 
            Gizmos.color = g %2 == 0 ? c : Color.clear;
            Gizmos.DrawLine(Vector3.Lerp(s,e, g * (1f / m)), Vector3.Lerp(s, e, (g+1) * (1f/ m)));
        } 
    }


#endif




}




[System.Serializable]
public class Vector3_StageBorder
{
    public Vector3 basePosition;
    [Space(5)]
    public float thickness;
    public Vector3 size;

    public static implicit operator StageBorderParameter(Vector3_StageBorder v)
    {
        return new StageBorderParameter()
        {
            minusVector = v.basePosition - v.size / 2,
            plusVector = v.basePosition + v.size / 2
        };
    }
}



[System.Serializable]
public class StageBorderParameter
{
    public Vector3 minusVector;
    public Vector3 plusVector; 
}

