/*-----------------------------------------------
   海の処理におけるメインのクラス
    
   
 ------------------------------------------------*/


using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif



public class Wave : MonoBehaviour
{ 
    [SerializeField] bool GO_MakePolygon;
    [Header("◆ポリゴン数の１辺の数")]
    [Space(30)]
    [SerializeField, Range(100, 256)]public int VerticesToVector = 100;


    [Header("◆波の頂点操作パラメータ")]
    [Space(30)]
    public float WaveSpeed = 1f;
    public float WaveHeight = 0.1f;
    public float WaveArea = 100f;
    public float WaveInterval = 0.1f;
    public float WaveRing = 1; 
    public float WaveRingHeight = 1;
    public float WhiteHeight = 1;
    [Space(2)]
    public float UnderSeaView_AdjustedValue = 1.31f;

    [Header("◆マテリアルの動作速度に関数パラメータ")] 
    [Space(30)]
    [SerializeField] VectorXZ Waveheight_PlaneAxis = new VectorXZ() { x = 1, z = 1 };


    [Header("◆マテリアルの動作速度に関数パラメータ")]
    [Space(30)]
    public float MaterialMoveMaster;
    public SeaMaterial[] MaterialMove;
    [Range(1, 4)]
    public float SePow = 1.4f;
    public SeaMaterial[] SeaShadow; 

    [Header("#水中描画対象カメラ-------------------")]
    public Camera targetCamera;
    //ReadOnlyArea------------------------
    [Header("#読み取り専用パラメータ-------------------")]
    [Space(30)]
    [SerializeField] float Scale;
    [SerializeField] float PolyCount;

    public float GetScale => Scale;

    //-------------------------------------- 
    float Cnt;
    Vector3 startPos;
    MeshFilter meshFilter;
    MeshRenderer meshRender;
    private Mesh mesh;
    List<Vector3> vertexList;
    private List<int> indexList;
    List<Vector2> uvList;
    AudioSource waveSe;
    int cssbcnt;
    bool SetListFlag = true;

    [System.NonSerialized]
    public MeshRenderer CastSeaShadowBaseObj;

    [System.NonSerialized]
    public List<MeshRenderer> CastSeaShadowBaseList = new List<MeshRenderer>();

    static float WaveUp;
    public static Wave wave;  
    public static List<Transform> bubbleList = new List<Transform>();

    public Transform[] L, C;
    //エディタ用変数
    const bool IsActive = false;
    static int Len, st, en, cn; 
    const int perFlame = 10;
     
    /// <summary>
    /// 波の角度を返す
    /// </summary>
    /// <param name="d">座標(y軸無視)</param>
    /// <param name="tar"></param>
    /// <returns></returns>
    public static Quaternion GetSurfaceNormal(Vector3 d)
    {
        float w = wave.VerticesToVector * wave.WaveArea / 2;
        Vector3 c1 = wave.transform.position - new Vector3(w / 2, 0, w / 2);
        Vector3 c2 = wave.transform.position + new Vector3(w / 2, 0, w / 2);
        float ux = (d.z - c1.z) / (c2.z - c1.z) * wave.VerticesToVector; //0~1で表記した際の対象ｙとなるｘ座標
        float uy = (d.x - c1.x) / (c2.x - c1.x) * wave.VerticesToVector;//0~1で表記した際の対象ｙとなるｙ座標

        int x = (int)ux;
        int y = (int)uy;
        int wx = x + 1;
        int wy = y + 1;
        if (x < 0) x = 0; else if (x >= wave.VerticesToVector) x = wave.VerticesToVector - 1;
        if (y < 0) y = 0; else if (y >= wave.VerticesToVector) y = wave.VerticesToVector - 1;
        if (wx < 0) wx = 0; else if (wx >= wave.VerticesToVector) wx = wave.VerticesToVector - 1;
        if (wy < 0) wy = 0; else if (wy >= wave.VerticesToVector) wy = wave.VerticesToVector - 1;



        Vector3[] pointHeight = new Vector3[4]
        {
            wave.vertexList[y * wave.VerticesToVector + x]  + Vector3.up* (WaveUp + Wave.wave.transform.position.y),
            wave.vertexList[y * wave.VerticesToVector + wx]  + Vector3.up * (WaveUp + Wave.wave.transform.position.y),
            wave.vertexList[wy * wave.VerticesToVector + x]  + Vector3.up * (WaveUp + Wave.wave.transform.position.y),
            wave.vertexList[wy * wave.VerticesToVector + wx]  + Vector3.up * (WaveUp + Wave.wave.transform.position.y)
        };

        Vector2 pointLen = new Vector2((ux - x) / (wx - x), (uy - y) / (wy - y));
        Vector3[] LineHeight = new Vector3[4]
        {
            pointHeight[0] + (pointHeight[1]-pointHeight[0])*pointLen.x,
            pointHeight[0] + (pointHeight[2]-pointHeight[0])*pointLen.y,
            pointHeight[2] + (pointHeight[3]-pointHeight[2])*pointLen.x,
            pointHeight[1] + (pointHeight[3]-pointHeight[1])*pointLen.y
        };

        var a = Vector3.Cross(pointHeight[1] - pointHeight[0], pointHeight[2] - pointHeight[0]).normalized;
        Vector2 za, zb;



        za = new Vector2(Mathf.Abs(LineHeight[2].x - LineHeight[0].x), LineHeight[2].y - LineHeight[0].y);
        zb = new Vector2(Mathf.Abs(LineHeight[3].x - LineHeight[1].x), LineHeight[3].y - LineHeight[1].y);

        float ta, tb;

        ta = Mathf.Atan(za.x / za.y);
        tb = Mathf.Atan(zb.x / zb.y);

        ta =  90-(ta * 180 / Mathf.PI);
        tb = 90 - (tb * 180 / Mathf.PI);


        return Quaternion.Euler(ta,0,tb);
        //float ez = Mathf.Asin(Mathf.Abs(pointHeight[1].y - pointHeight[2].y) / Vector3.Distance(pointHeight[1], pointHeight[2]));
        //float ex = Mathf.Asin(Mathf.Abs(pointHeight[0].y - pointHeight[3].y) / Vector3.Distance(pointHeight[0], pointHeight[3]));

        return Quaternion.Euler
        (
            (GetNormal(pointHeight[0], pointHeight[1], pointHeight[2]) +
            GetNormal(pointHeight[0], pointHeight[1], pointHeight[2]))
            *-0.5f
         );

        //return  Quaternion.Euler(ex*-90, 0, ez*90);
        /*こんな位置関係
                  x         wx
                  |          |
               y-[0]--------[1]--
                  |          |
                  |          |
                  |          |
                  |          |
                  |          |
              wy-[2]--------[3]-
                  |          |

     
      [?] = pointHeight
      [0]と[3]，[1]と[2]の高度差と距離を用いて計算する

       */

    }

    static Vector3 GetNormal(Vector3 v0, Vector3 v1, Vector3 v2)
    { 
        Vector3 a = new Vector3(v1.x - v0.x, v1.y - v0.y, v1.z - v0.z);
        Vector3 b = new Vector3(v2.x - v0.x, v2.y - v0.y, v2.z - v0.z);   
        return new Vector3 (a.y*b.z-a.z*b.y, a.z*b.x-a.x*b.z, a.x*b.y-a.y*b.x);
    }

    /// 波の角度を返す
    /// </summary>
    /// <param name="d">座標(y軸無視)</param> 
    /// <returns></returns>
    public static float GetSurfaceHeight(Vector3 d, bool deb = false)
    {
        float w = wave.VerticesToVector * wave.WaveArea / 2;
        Vector3 c1 = wave.transform.position - new Vector3(w / 2, 0, w / 2);
        Vector3 c2 = wave.transform.position + new Vector3(w / 2, 0, w / 2);
        float ux = (d.z - c1.z) / (c2.z - c1.z) * wave.VerticesToVector; //0~1で表記した際の対象ｙとなるｘ座標
        float uy = (d.x - c1.x) / (c2.x - c1.x) * wave.VerticesToVector;//0~1で表記した際の対象ｙとなるｙ座標

        int x = (int)ux;
        int y = (int)uy;
        int wx = x + 1;
        int wy = y + 1;
        if (x < 0) x = 0; else if (x >= wave.VerticesToVector) x = wave.VerticesToVector - 1;
        if (y < 0) y = 0; else if (y >= wave.VerticesToVector) y = wave.VerticesToVector - 1;
        if (wx < 0) wx = 0; else if (wx >= wave.VerticesToVector) wx = wave.VerticesToVector - 1;
        if (wy < 0) wy = 0; else if (wy >= wave.VerticesToVector) wy = wave.VerticesToVector - 1;
       
        

        Vector3[] pointHeight = new Vector3[4]
        {
            wave.vertexList[y * wave.VerticesToVector + x] + Vector3.up * (WaveUp + Wave.wave.transform.position.y),
            wave.vertexList[y * wave.VerticesToVector + wx] + Vector3.up * (WaveUp + Wave.wave.transform.position.y),
            wave.vertexList[wy * wave.VerticesToVector + x] + Vector3.up * (WaveUp + Wave.wave.transform.position.y),
            wave.vertexList[wy * wave.VerticesToVector + wx] + Vector3.up * (WaveUp + Wave.wave.transform.position.y)
        };

        

        Vector2 pointLen = new Vector2((ux - x) / (wx - x), (uy - y) / (wy - y));

        Vector3[] LineHeight = new Vector3[4]
        {
            pointHeight[0] + (pointHeight[1]-pointHeight[0])*pointLen.x,
            pointHeight[0] + (pointHeight[2]-pointHeight[0])*pointLen.y,
            pointHeight[2] + (pointHeight[3]-pointHeight[2])*pointLen.x,
            pointHeight[1] + (pointHeight[3]-pointHeight[1])*pointLen.y
        };
        if (deb)
        { 
        wave.L[0].position = LineHeight[0];
        wave.L[1].position = LineHeight[1];
        wave.L[2].position = LineHeight[2];
        wave.L[3].position = LineHeight[3];
        wave.C[0].position = pointHeight[0];
        wave.C[1].position = pointHeight[1];
        wave.C[2].position = pointHeight[2];
        wave.C[3].position = pointHeight[3];
        }

        // Vector3 AveX = LineHeight[0] + (LineHeight[2] - LineHeight[0]) * pointLen.y;
        // Vector3 AveY = LineHeight[1] + (LineHeight[3] - LineHeight[1]) * pointLen.x;

        /*こんな位置関係
                    x         wx
                    |  LH[0]   |
                 y-[0]---X----[1]--
                    |    |     |
                    |    |     |
             LH[1]  Y----T-----| LH[3]
                    |    |     |
                    |    |     |
                wy-[2]--------[3]-
                    |  LH[2]   |

        LH = LineHeight
        [?] = pointHeight
        X,Y = pointLen
        
        LH[0]とLH[2]のY交差点の高度をAveX,
        LH[3]とLH[1]のX交差点の高度をAveYとする
        AveXとAveYの値は実質同じ（もしくは差は極小的なもの）であるため
        AveXとAveYの値の平均に海面フェースを座標的に動かしているWaveUp変数の値を足したものを
        高度として使用する。
         */
        //return (pointHeight[3] - pointHeight[0]).y*(Vector2.Distance(new Vector2(d.x,d.z), new Vector2(pointHeight[0].x, pointHeight[0].z)/ Vector2.Distance(new Vector2(pointHeight[3].x, pointHeight[3].z), new Vector2(pointHeight[0].x, pointHeight[0].z)))) + pointHeight[0].y;
        //pointHeight/pointLen
        float Ay = pointHeight[0].y;
        float By = pointHeight[1].y;
        float Cy = pointHeight[2].y;
        float Dy = pointHeight[3].y;
        float W = pointLen.x;
        float H = pointLen.y;
        return (((Dy-Cy)*W+Cy)   -  ((By-Ay)*W+Ay))*H + ((By - Ay) * W + Ay);
        return ((LineHeight[0] + LineHeight[1] + LineHeight[2] + LineHeight[3]) / 4).y;
    }



    //---------------------------------------//
    //エディタ―関連の処理-------------------//
    //---------------------------------------//

#if UNITY_EDITOR

    /// <summary>
    /// この変数はエディタ拡張用の変数です
    /// </summary>
    static int editorCnt;
 

    void OnValidate()
    {
        if (!EditorApplication.isPlaying)
        { 
           MakeMesh();
           EDITORUPDATE();
        }
    }

    private void OnDrawGizmos()
    {
        if (!EditorApplication.isPlaying && GO_MakePolygon)
        {
            GO_MakePolygon = false;
            MakeMesh();
            EDITORUPDATE();
            Debug.Log("Update Wave Poly!! ");
        }
    }

    //水中にあるオブジェクトに影を付ける
    static void EDITORUPDATE()
    { 
        if (GameObject.Find("Sea") == null) return;
        if (wave == null) return;

        if (EditorApplication.isPlaying) return;
        wave.transform.Find("Canvas_UnderSeaView").GetComponent<Canvas>().worldCamera = wave.targetCamera;

        Len = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)).Length;
        cn++; 
        st = cn * Len/ perFlame;
        en = (cn+1) * Len / perFlame;
        if (en >= Len)
        {
            en = Len - 1;
            cn = 0;
        }

        long j = 0; 
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        { 
            if (st <= j && j < en)
            {
                if (obj.activeInHierarchy)
                {
                    if (obj.GetComponent<CastSeaShadowObject>() != null)
                    {
                        DestroyImmediate(obj.GetComponent<CastSeaShadowObject>());
                    }
                }
            } 
            j++;
        } 
    }
     
#endif

    /// <summary>
    /// wave変数をセットする
    /// </summary>
    void SetWaveComponent()
    {
        var w = GameObject.Find("Sea");
        if (w != null)
        {
            if (wave   == null) wave   = w.GetComponent<Wave>();
            if (waveSe == null) waveSe = w.GetComponent<AudioSource>();
        }
    }

    /// <summary>
    /// 波のメッシュ生成
    /// </summary>
    void MakeMesh()
    { 
        SetWaveComponent();
        transform.localScale = new Vector3(1, 1, 1);
        meshFilter = GetComponent<MeshFilter>();
        meshRender = GetComponent<MeshRenderer>(); 
        mesh = CreatePlaneMesh(); 
        meshFilter.mesh = mesh;
        Scale = VerticesToVector * WaveArea/2;
        PolyCount = VerticesToVector * VerticesToVector;
    }



    /// <summary>
    /// MakeMeshで呼ばれる
    /// </summary>
    /// <returns></returns>
    private Mesh CreatePlaneMesh()
    {
        var mesh = new Mesh();
        vertexList = new List<Vector3>();
        indexList = new List<int>();
        List<Vector2> uvList = new List<Vector2>();

        for (int x = 0; x < VerticesToVector; x++)
        {
            for (int z = 0; z < VerticesToVector; z++)
            {
                Vector3 f = new Vector3(x - VerticesToVector / 2, 0, z - VerticesToVector / 2) * WaveArea / 2;
                vertexList.Add(transform.position + f);
                uvList.Add(new Vector2(x * 1f / VerticesToVector, z * 1f / VerticesToVector));
            }
        }

        mesh.SetVertices(vertexList);//meshに頂点群をセット
        mesh.SetUVs(0, uvList);

        //サブメッシュの設定----------------------------------------------------
        for (int x = 0; x < VerticesToVector - 1; x++)
        {
            for (int z = 0; z < VerticesToVector - 1; z++)
            {
                //0,2,1,1,2,3の順
                int V0 = x + (z * VerticesToVector);
                int V1 = V0 + 1;
                int V2 = V0 + VerticesToVector;
                int V3 = V2 + 1;
                indexList.Add(V0);
                indexList.Add(V2);
                indexList.Add(V1);
                indexList.Add(V1);
                indexList.Add(V2);
                indexList.Add(V3);
            }
        }

        mesh.SetIndices(indexList.ToArray(), MeshTopology.Triangles, 0);//メッシュにどの頂点の順番で面を作るかセット

        return mesh;
    }


    



    private void Awake()
    {
        MakeMesh();
        bubbleList = new List<Transform>();
        CastSeaShadowBaseList = new List<MeshRenderer>();
        CastSeaShadowBaseObj = null;
    }




    private void Start()
    { 
        meshFilter = GetComponent<MeshFilter>();
        meshRender = GetComponent<MeshRenderer>();
        wave = null;
        SetWaveComponent(); 
        startPos = transform.position;
        Material[] a = new Material[MaterialMove.Length];
        for (int g = 0; g < a.Length; g++)
            a[g] = MaterialMove[g].material;
        meshRender.materials = a;
        SetListFlag = true;
        SetWaveComponent();
        transform.position = new Vector3(startPos.x, startPos.y + WaveUp, startPos.z);
    }



    public void Update()
    { 
        if (CastSeaShadowBaseList != null && SetListFlag && CastSeaShadowBaseList.Count > 0)
        {
            SetListFlag = false;
            cssbcnt = CastSeaShadowBaseObj.sharedMaterials.Length - SeaShadow.Length;
            CastSeaShadowBaseList = null;    
        } 
         
         
        waveSe.volume = Mathf.Pow((Vector3.Distance(new Vector3(wave.targetCamera.transform.position.x,0, wave.targetCamera.transform.position.z),Vector3.zero)/ Mathf.Abs(wave.targetCamera.transform.position.y/10) / (Scale / 2)), SePow);
        WaveUp = WaveHeight / 22 * Mathf.Sin(Cnt);
        Cnt += Time.deltaTime * WaveSpeed;
        Debug.Log(Sin(Cnt));
        float ccx = Cnt * WaveInterval * Mathf.PI;
        float cntSIN = WaveInterval * Mathf.Sin(Cnt) * WaveRing * Mathf.PI;

        for (int x = 0; x < VerticesToVector - 1; x++)
          {
              for (int z = 0; z < VerticesToVector - 1; z++)
              {
                
                   int i = z * VerticesToVector + x;
                   var v = vertexList[i];

                   float d = ((float)(x - VerticesToVector / 2) * (x - VerticesToVector / 2) + (float)(z - VerticesToVector / 2) * (z - VerticesToVector / 2));
                   float sinx = Sin(ccx*x);
                   float sinz = Sin(ccx*z);  
                   v.y =
                   WaveHeight * (d / VerticesToVector + 1) *
                   ( 
                        sinx * sinx * Waveheight_PlaneAxis.x +
                        sinz * sinz * Waveheight_PlaneAxis.z + 
                        Sin((d + 1) * cntSIN) * WaveRingHeight
                   );
                   vertexList[i] = v; 
            }
        }
       mesh.SetVertices(vertexList); 
         
       
        //マテリアル設定－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－
        int cc = meshRender.materials.Length >= MaterialMove.Length ? meshRender.materials.Length : MaterialMove.Length;
        for (int e = 0; e < cc; e++)
            meshRender.materials[e].mainTextureOffset += MaterialMoveMaster * MaterialMove[e].move * Time.deltaTime;
        //泡がらみ 
        if (GetSurfaceHeight(wave.targetCamera.transform.position) > wave.targetCamera.transform.position.y)
        { 
            foreach (Transform t in bubbleList.ToArray())
                t.gameObject.active = Vector3.Distance(wave.targetCamera.transform.position, t.position) < 50;
        }
        else
        {
            foreach (Transform t in bubbleList.ToArray())
                t.gameObject.active = false;
        }


        //影がらみ
        if (CastSeaShadowBaseList != null && CastSeaShadowBaseList.Count > 0)
        {
            for (int c = 0; c < SeaShadow.Length; c++)
            {
                CastSeaShadowBaseObj.sharedMaterials[c+ cssbcnt].mainTextureOffset += SeaShadow[c].move;
            }
        } 
    }

    static float Sin(float s) => sinArray[(int)(s % 1f * 999)];

    #region さいんのはいれつ
    static float[] sinArray = new float[1000]
 {
    0f,//raw0:0
    0.00314158763509647f,//raw1:0.001
    0.00628314426398835f,//raw2:0.002
    0.00942463851506378f,//raw3:0.003
    0.0125660404801761f,//raw4:0.004
    0.0157073184235013f,//raw5:0.005
    0.0188484398792259f,//raw6:0.006
    0.021989376771135f,//raw7:0.007
    0.0251300966367016f,//raw8:0.008
    0.0282705684782786f,//raw9:0.009000001
    0.0314107613006665f,//raw10:0.01
    0.0345506441114198f,//raw11:0.011
    0.0376901829973953f,//raw12:0.012
    0.0408293528204482f,//raw13:0.013
    0.04396811967414f,//raw14:0.014
    0.0471064525800991f,//raw15:0.015
    0.0502443205642364f,//raw16:0.016
    0.0533816926570516f,//raw17:0.017
    0.0565185378939387f,//raw18:0.018
    0.0596548253154913f,//raw19:0.019
    0.0627905239678087f,//raw20:0.02
    0.0659256029028009f,//raw21:0.021
    0.0690600311784944f,//raw22:0.022
    0.0721937778593374f,//raw23:0.023
    0.075326806181458f,//raw24:0.024
    0.0784590968945717f,//raw25:0.025
    0.0815906132478203f,//raw26:0.026
    0.0847213243343917f,//raw27:0.027
    0.0878511992554212f,//raw28:0.028
    0.0909802071202968f,//raw29:0.029
    0.0941083170469638f,//raw30:0.03
    0.0972354981622303f,//raw31:0.031
    0.100361719602071f,//raw32:0.032
    0.103486944691678f,//raw33:0.033
    0.106611160047036f,//raw34:0.034
    0.109734311556352f,//raw35:0.035
    0.11285639166457f,//raw36:0.036
    0.115977346296881f,//raw37:0.037
    0.119097167903542f,//raw38:0.038
    0.122215802448861f,//raw39:0.039
    0.12533324238871f,//raw40:0.04
    0.128449433728647f,//raw41:0.041
    0.131564368930452f,//raw42:0.042
    0.134677994043067f,//raw43:0.043
    0.13779030153448f,//raw44:0.044
    0.140901237499145f,//raw45:0.045
    0.144010794411555f,//raw46:0.046
    0.147118918413804f,//raw47:0.047
    0.150225590416657f,//raw48:0.048
    0.153330791323569f,//raw49:0.049
    0.156434467352082f,//raw50:0.05
    0.159536610994252f,//raw51:0.051
    0.162637168520571f,//raw52:0.052
    0.165736132430639f,//raw53:0.053
    0.168833449050021f,//raw54:0.054
    0.171929110886158f,//raw55:0.055
    0.175023064321807f,//raw56:0.056
    0.178115301872544f,//raw57:0.057
    0.181205769980436f,//raw58:0.058
    0.184294461169489f,//raw59:0.059
    0.187381321943194f,//raw60:0.06
    0.190466344834284f,//raw61:0.061
    0.193549476409786f,//raw62:0.062
    0.196630697736584f,//raw63:0.063
    0.199709989871953f,//raw64:0.064
    0.20278731094236f,//raw65:0.06500001
    0.205862607670425f,//raw66:0.066
    0.208935895529791f,//raw67:0.067
    0.212007121267862f,//raw68:0.068
    0.215076254572755f,//raw69:0.06900001
    0.218143242310262f,//raw70:0.07
    0.221208099912217f,//raw71:0.071
    0.224270774270683f,//raw72:0.072
    0.227331235158176f,//raw73:0.07300001
    0.230389429592048f,//raw74:0.074
    0.23344537295989f,//raw75:0.075
    0.236499012306807f,//raw76:0.07600001
    0.239550317494491f,//raw77:0.07700001
    0.242599235700217f,//raw78:0.078
    0.245645782264903f,//raw79:0.079
    0.248689904395062f,//raw80:0.08000001
    0.251731549393391f,//raw81:0.081
    0.25477073256421f,//raw82:0.082
    0.257807401240563f,//raw83:0.083
    0.260841525451634f,//raw84:0.08400001
    0.263873052674624f,//raw85:0.085
    0.266901998162947f,//raw86:0.086
    0.269928309425624f,//raw87:0.087
    0.272951956594063f,//raw88:0.08800001
    0.275972887328267f,//raw89:0.089
    0.278991116828331f,//raw90:0.09
    0.282006592787545f,//raw91:0.09100001
    0.285019285444257f,//raw92:0.09200001
    0.288029142649536f,//raw93:0.093
    0.291036179547758f,//raw94:0.094
    0.294040344024742f,//raw95:0.09500001
    0.297041584080259f,//raw96:0.096
    0.30003991481533f,//raw97:0.097
    0.30303528426557f,//raw98:0.098
    0.306027662867772f,//raw99:0.09900001
    0.309016998827164f,//raw100:0.1
    0.312003307184869f,//raw101:0.101
    0.314986536183412f,//raw102:0.102
    0.317966656379408f,//raw103:0.103
    0.320943616191719f,//raw104:0.104
    0.323917430599196f,//raw105:0.105
    0.326888048059446f,//raw106:0.106
    0.329855439253556f,//raw107:0.107
    0.332819552822166f,//raw108:0.108
    0.335780403679493f,//raw109:0.109
    0.338737940506363f,//raw110:0.11
    0.341692134112964f,//raw111:0.111
    0.344642933369827f,//raw112:0.112
    0.347590353124176f,//raw113:0.113
    0.350534342288163f,//raw114:0.114
    0.353474849910038f,//raw115:0.115
    0.356411890785241f,//raw116:0.116
    0.359345414004713f,//raw117:0.117
    0.362275390615648f,//raw118:0.118
    0.36520176991029f,//raw119:0.119
    0.368124566612991f,//raw120:0.12
    0.371043730060112f,//raw121:0.121
    0.37395923144057f,//raw122:0.122
    0.376871020298614f,//raw123:0.123
    0.379779111285181f,//raw124:0.124
    0.38268343236509f,//raw125:0.125
    0.385583998151703f,//raw126:0.126
    0.388480758364496f,//raw127:0.127
    0.3913736844135f,//raw128:0.128
    0.394262747746587f,//raw129:0.129
    0.397147919849753f,//raw130:0.13
    0.400029172247397f,//raw131:0.131
    0.402906433657077f,//raw132:0.132
    0.405779761431374f,//raw133:0.133
    0.408649084307013f,//raw134:0.134
    0.411514373964821f,//raw135:0.135
    0.414375602125431f,//raw136:0.136
    0.417232740549563f,//raw137:0.137
    0.420085761038298f,//raw138:0.138
    0.422934635433363f,//raw139:0.139
    0.425779293259393f,//raw140:0.14
    0.428619791219081f,//raw141:0.141
    0.431456058857324f,//raw142:0.142
    0.434288068181192f,//raw143:0.143
    0.437115791239785f,//raw144:0.144
    0.439939200124502f,//raw145:0.145
    0.442758266969327f,//raw146:0.146
    0.445572922041621f,//raw147:0.147
    0.448383221446031f,//raw148:0.148
    0.451189095471128f,//raw149:0.149
    0.453990516423957f,//raw150:0.15
    0.456787456655511f,//raw151:0.151
    0.459579888561008f,//raw152:0.152
    0.462367784580161f,//raw153:0.153
    0.465151117197449f,//raw154:0.154
    0.467929817570338f,//raw155:0.155
    0.470703941086788f,//raw156:0.156
    0.473473418926541f,//raw157:0.157
    0.476238223755857f,//raw158:0.158
    0.47899832828712f,//raw159:0.159
    0.481753705279101f,//raw160:0.16
    0.484504327537231f,//raw161:0.161
    0.487250127033504f,//raw162:0.162
    0.489991158500065f,//raw163:0.163
    0.492727353932109f,//raw164:0.164
    0.495458686324381f,//raw165:0.165
    0.498185128719624f,//raw166:0.166
    0.500906654208843f,//raw167:0.167
    0.503623235931572f,//raw168:0.168
    0.506334847076137f,//raw169:0.169
    0.50904142058568f,//raw170:0.17
    0.511743010410455f,//raw171:0.171
    0.514439549517845f,//raw172:0.172
    0.517131011293989f,//raw173:0.173
    0.51981736917514f,//raw174:0.174
    0.522498596647922f,//raw175:0.175
    0.525174667249595f,//raw176:0.176
    0.527845514807822f,//raw177:0.177
    0.530511192560729f,//raw178:0.178
    0.53317163436112f,//raw179:0.179
    0.535826813951401f,//raw180:0.18
    0.538476705125916f,//raw181:0.181
    0.541121281731203f,//raw182:0.182
    0.543760517666253f,//raw183:0.183
    0.546394386882766f,//raw184:0.184
    0.549022824258446f,//raw185:0.185
    0.551645882186055f,//raw186:0.186
    0.554263495569447f,//raw187:0.187
    0.556875638573732f,//raw188:0.188
    0.559482285418007f,//raw189:0.189
    0.562083410375616f,//raw190:0.19
    0.564678987774403f,//raw191:0.191
    0.567268953444619f,//raw192:0.192
    0.569853359012168f,//raw193:0.193
    0.572432140334327f,//raw194:0.194
    0.575005271959462f,//raw195:0.195
    0.5775727284917f,//raw196:0.196
    0.580134484591179f,//raw197:0.197
    0.582690514974298f,//raw198:0.198
    0.585240794413967f,//raw199:0.199
    0.587785259867037f,//raw200:0.2
    0.590323962052453f,//raw201:0.201
    0.592856837955073f,//raw202:0.202
    0.595383862576333f,//raw203:0.203
    0.597905010975418f,//raw204:0.204
    0.600420258269511f,//raw205:0.205
    0.602929579634036f,//raw206:0.206
    0.605432950302904f,//raw207:0.207
    0.607930308399385f,//raw208:0.208
    0.61042170370343f,//raw209:0.209
    0.612907074367279f,//raw210:0.21
    0.615386395861226f,//raw211:0.211
    0.61785964371527f,//raw212:0.212
    0.620326793519355f,//raw213:0.213
    0.62278782092361f,//raw214:0.214
    0.625242665104005f,//raw215:0.215
    0.627691374993062f,//raw216:0.216
    0.630133889796547f,//raw217:0.217
    0.632570185407728f,//raw218:0.218
    0.635000237781254f,//raw219:0.219
    0.637424022933391f,//raw220:0.22
    0.639841516942262f,//raw221:0.221
    0.642252695948079f,//raw222:0.222
    0.64465750036589f,//raw223:0.223
    0.647055978130769f,//raw224:0.224
    0.649448069688487f,//raw225:0.225
    0.65183375142997f,//raw226:0.226
    0.654212999809408f,//raw227:0.227
    0.656585791344484f,//raw228:0.228
    0.658952102616608f,//raw229:0.229
    0.661311875155917f,//raw230:0.23
    0.663665155999864f,//raw231:0.231
    0.666011886710105f,//raw232:0.232
    0.668352044125262f,//raw233:0.233
    0.670685605148833f,//raw234:0.234
    0.673012546749418f,//raw235:0.235
    0.67533284596095f,//raw236:0.236
    0.677646479882921f,//raw237:0.237
    0.679953391354446f,//raw238:0.238
    0.682253626359296f,//raw239:0.239
    0.684547127768998f,//raw240:0.24
    0.686833872947529f,//raw241:0.241
    0.689113839325544f,//raw242:0.242
    0.691387004400606f,//raw243:0.243
    0.693653345737401f,//raw244:0.244
    0.695912807350048f,//raw245:0.245
    0.698165434276497f,//raw246:0.246
    0.700411170564043f,//raw247:0.247
    0.702649994048084f,//raw248:0.248
    0.704881882632247f,//raw249:0.249
    0.707106781186547f,//raw250:0.25
    0.709324767057888f,//raw251:0.251
    0.711535686156295f,//raw252:0.252
    0.713739648442786f,//raw253:0.253
    0.715936500801645f,//raw254:0.254
    0.718126352494276f,//raw255:0.255
    0.720309051385266f,//raw256:0.256
    0.722484641299774f,//raw257:0.257
    0.724653165285649f,//raw258:0.258
    0.726814472689178f,//raw259:0.259
    0.728968671003919f,//raw260:0.26
    0.731115610571408f,//raw261:0.261
    0.733255398170611f,//raw262:0.262
    0.735387885144217f,//raw263:0.263
    0.737513114323171f,//raw264:0.264
    0.739631127744838f,//raw265:0.265
    0.741741778264621f,//raw266:0.266
    0.743845170856638f,//raw267:0.267
    0.745941159392045f,//raw268:0.268
    0.748029848115738f,//raw269:0.269
    0.750111091920394f,//raw270:0.27
    0.752184994317574f,//raw271:0.271
    0.754251411227205f,//raw272:0.272
    0.75631038417159f,//raw273:0.273
    0.758361953859098f,//raw274:0.274
    0.760405977761174f,//raw275:0.275
    0.762442557538231f,//raw276:0.276
    0.764471551703107f,//raw277:0.277
    0.766493061168705f,//raw278:0.278
    0.768506945494841f,//raw279:0.279
    0.770513245162987f,//raw280:0.28
    0.772511999825293f,//raw281:0.281
    0.77450307062242f,//raw282:0.282
    0.776486556581136f,//raw283:0.283
    0.77846231990286f,//raw284:0.284
    0.780430458853036f,//raw285:0.285
    0.782390836699288f,//raw286:0.286
    0.784343492862677f,//raw287:0.287
    0.786288465919962f,//raw288:0.288
    0.788225620748168f,//raw289:0.289
    0.790155053692528f,//raw290:0.29
    0.792076630709686f,//raw291:0.291
    0.793990447370105f,//raw292:0.292
    0.795896370715336f,//raw293:0.293
    0.797794439086968f,//raw294:0.294
    0.799684689967713f,//raw295:0.295
    0.801566992036304f,//raw296:0.296
    0.803441438909851f,//raw297:0.297
    0.805307900365039f,//raw298:0.298
    0.807166469231135f,//raw299:0.299
    0.809017016387918f,//raw300:0.3
    0.81085963387315f,//raw301:0.301
    0.812694193674798f,//raw302:0.302
    0.814520732719385f,//raw303:0.303
    0.816339287056429f,//raw304:0.304
    0.818149730345634f,//raw305:0.305
    0.819952152630181f,//raw306:0.306
    0.821746428690514f,//raw307:0.307
    0.823532647765729f,//raw308:0.308
    0.825310685761957f,//raw309:0.309
    0.827080578484645f,//raw310:0.31
    0.828842360848627f,//raw311:0.311
    0.830595910457719f,//raw312:0.312
    0.832341314525473f,//raw313:0.313
    0.834078451793553f,//raw314:0.314
    0.835807408659235f,//raw315:0.315
    0.837528065006821f,//raw316:0.316
    0.839240455503371f,//raw317:0.317
    0.840944613912147f,//raw318:0.318
    0.842640421840271f,//raw319:0.319
    0.844327963629484f,//raw320:0.32
    0.846007122041316f,//raw321:0.321
    0.847677980589438f,//raw322:0.322
    0.849340423194411f,//raw323:0.323
    0.850994483368145f,//raw324:0.324
    0.852640193705999f,//raw325:0.325
    0.854277439875656f,//raw326:0.326
    0.855906303306262f,//raw327:0.327
    0.857526670835905f,//raw328:0.328
    0.859138623054264f,//raw329:0.329
    0.860742047974298f,//raw330:0.33
    0.862337025343038f,//raw331:0.331
    0.863923444352736f,//raw332:0.332
    0.865501337006369f,//raw333:0.333
    0.867070734374629f,//raw334:0.334
    0.868631527426884f,//raw335:0.335
    0.870183793789852f,//raw336:0.336
    0.871727425623042f,//raw337:0.337
    0.873262499699651f,//raw338:0.338
    0.874788909373588f,//raw339:0.339
    0.876306685456468f,//raw340:0.34
    0.877815857815896f,//raw341:0.341
    0.879316321605248f,//raw342:0.342
    0.880808151450739f,//raw343:0.343
    0.882291243710582f,//raw344:0.344
    0.883765672147011f,//raw345:0.345
    0.885231334327164f,//raw346:0.346
    0.886688259856396f,//raw347:0.347
    0.88813647738478f,//raw348:0.348
    0.889575886300404f,//raw349:0.349
    0.891006558192892f,//raw350:0.35
    0.892428393669278f,//raw351:0.351
    0.893841463445164f,//raw352:0.352
    0.895245669350455f,//raw353:0.353
    0.896641039769925f,//raw354:0.354
    0.898027602122264f,//raw355:0.355
    0.899405260078923f,//raw356:0.356
    0.900774082158317f,//raw357:0.357
    0.902133973264368f,//raw358:0.358
    0.903485001031847f,//raw359:0.359
    0.904827071600899f,//raw360:0.36
    0.906160251719984f,//raw361:0.361
    0.907484448769174f,//raw362:0.362
    0.908799689543843f,//raw363:0.363
    0.910105999860068f,//raw364:0.364
    0.911403288964646f,//raw365:0.365
    0.912691621378813f,//raw366:0.366
    0.913970907598341f,//raw367:0.367
    0.915241211249084f,//raw368:0.368
    0.916502444079328f,//raw369:0.369
    0.917754631633368f,//raw370:0.37
    0.918997798466477f,//raw371:0.371
    0.920231858212212f,//raw372:0.372
    0.92145687224707f,//raw373:0.373
    0.922672755465737f,//raw374:0.374
    0.923879568340691f,//raw375:0.375
    0.925077227031108f,//raw376:0.376
    0.926265755817855f,//raw377:0.377
    0.927445177983521f,//raw378:0.378
    0.928615411590193f,//raw379:0.379
    0.929776514839947f,//raw380:0.38
    0.930928407067539f,//raw381:0.381
    0.932071145562827f,//raw382:0.382
    0.933204650936421f,//raw383:0.383
    0.934328946194017f,//raw384:0.384
    0.935444053334264f,//raw385:0.385
    0.936549894887401f,//raw386:0.386
    0.937646525852938f,//raw387:0.387
    0.938733870044708f,//raw388:0.388
    0.939811981542247f,//raw389:0.389
    0.94088078544599f,//raw390:0.39
    0.941940334913361f,//raw391:0.391
    0.942990556334357f,//raw392:0.392
    0.944031471058907f,//raw393:0.393
    0.945063099419331f,//raw394:0.394
    0.946085369745386f,//raw395:0.395
    0.947098332880129f,//raw396:0.396
    0.948101918450043f,//raw397:0.397
    0.949096176368891f,//raw398:0.398
    0.950081037562664f,//raw399:0.399
    0.951056522081605f,//raw400:0.4
    0.952022648950699f,//raw401:0.401
    0.952979351050281f,//raw402:0.402
    0.953926675962344f,//raw403:0.403
    0.954864557873453f,//raw404:0.404
    0.955793043429569f,//raw405:0.405
    0.956712068126083f,//raw406:0.406
    0.957621650703855f,//raw407:0.407
    0.958521808871333f,//raw408:0.408
    0.959412480091891f,//raw409:0.409
    0.960293708663448f,//raw410:0.41
    0.961165433364475f,//raw411:0.411
    0.962027697550576f,//raw412:0.412
    0.962880441317733f,//raw413:0.413
    0.963723707077525f,//raw414:0.414
    0.964557436245812f,//raw415:0.415
    0.965381645866736f,//raw416:0.416
    0.966196351943557f,//raw417:0.417
    0.967001497876258f,//raw418:0.418
    0.967797123709119f,//raw419:0.419
    0.968583174167685f,//raw420:0.42
    0.969359688346515f,//raw421:0.421
    0.970126612298894f,//raw422:0.422
    0.970883961739807f,//raw423:0.423
    0.971631751337343f,//raw424:0.424
    0.972369929140369f,//raw425:0.425
    0.973098531862478f,//raw426:0.426
    0.973817508885504f,//raw427:0.427
    0.974526895968081f,//raw428:0.428
    0.975226643827011f,//raw429:0.429
    0.975916766840518f,//raw430:0.43
    0.976597278334429f,//raw431:0.431
    0.977268131031619f,//raw432:0.432
    0.977929358297236f,//raw433:0.433
    0.978580914193886f,//raw434:0.434
    0.979222831126983f,//raw435:0.435
    0.979855064500677f,//raw436:0.436
    0.980477627349481f,//raw437:0.437
    0.981090531650516f,//raw438:0.438
    0.981693734823525f,//raw439:0.439
    0.982287266869077f,//raw440:0.44
    0.982871086552765f,//raw441:0.441
    0.983445222911112f,//raw442:0.442
    0.984009636057185f,//raw443:0.443
    0.984564354062314f,//raw444:0.444
    0.985109338388616f,//raw445:0.445
    0.985644600333801f,//raw446:0.446
    0.986170150132518f,//raw447:0.447
    0.986685951273284f,//raw448:0.448
    0.98719202940969f,//raw449:0.449
    0.987688349383007f,//raw450:0.45
    0.988174935877924f,//raw451:0.451
    0.988651755089851f,//raw452:0.452
    0.989118816959338f,//raw453:0.453
    0.989576130360053f,//raw454:0.454
    0.990023663521092f,//raw455:0.455
    0.990461438700712f,//raw456:0.456
    0.990889425485388f,//raw457:0.457
    0.991307645161229f,//raw458:0.458
    0.991716068673289f,//raw459:0.459
    0.99211470460015f,//raw460:0.46
    0.992503560450204f,//raw461:0.461
    0.992882609208496f,//raw462:0.462
    0.993251869727181f,//raw463:0.463
    0.993611316352647f,//raw464:0.464
    0.99396096696213f,//raw465:0.465
    0.994300797264377f,//raw466:0.466
    0.994630814471854f,//raw467:0.467
    0.994951024724048f,//raw468:0.468
    0.995261405775015f,//raw469:0.469
    0.99556197306169f,//raw470:0.47
    0.995852705702784f,//raw471:0.471
    0.996133618158011f,//raw472:0.472
    0.99640469091156f,//raw473:0.473
    0.996665937445351f,//raw474:0.474
    0.996917339609822f,//raw475:0.475
    0.997158902855972f,//raw476:0.476
    0.997390631558995f,//raw477:0.477
    0.997612509620021f,//raw478:0.478
    0.997824548074249f,//raw479:0.479
    0.998026732190748f,//raw480:0.48
    0.998219072025233f,//raw481:0.481
    0.998401554215291f,//raw482:0.482
    0.998574182838806f,//raw483:0.483
    0.998736960896256f,//raw484:0.484
    0.998889877078973f,//raw485:0.485
    0.999032938992309f,//raw486:0.486
    0.999166136697334f,//raw487:0.487
    0.999289476818694f,//raw488:0.488
    0.999402950787589f,//raw489:0.489
    0.999506561306815f,//raw490:0.49
    0.999600310000684f,//raw491:0.491
    0.999684190356198f,//raw492:0.492
    0.999758206545184f,//raw493:0.493
    0.999822353425521f,//raw494:0.494
    0.99987663418758f,//raw495:0.495
    0.999921045060319f,//raw496:0.496
    0.999955587370139f,//raw497:0.497
    0.999980261364399f,//raw498:0.498
    0.999995065328924f,//raw499:0.499
    1f,//raw500:0.5
    0.999995064740641f,//raw501:0.501
    0.999980260187856f,//raw502:0.502
    0.999955586487739f,//raw503:0.503
    0.999921043883803f,//raw504:0.504
    0.99987663271696f,//raw505:0.505
    0.999822349896077f,//raw506:0.506
    0.999758202427597f,//raw507:0.507
    0.999684188003349f,//raw508:0.508
    0.999600307353806f,//raw509:0.509
    0.999506555425028f,//raw510:0.5100001
    0.999402944317855f,//raw511:0.511
    0.999289469761078f,//raw512:0.512
    0.999166132874615f,//raw513:0.513
    0.999032934875721f,//raw514:0.514
    0.998889868258115f,//raw515:0.515
    0.998736951487829f,//raw516:0.516
    0.99857417784085f,//raw517:0.517
    0.998401548923646f,//raw518:0.518
    0.998219055269363f,//raw519:0.5190001
    0.998026720432997f,//raw520:0.52
    0.997824535729451f,//raw521:0.521
    0.997612503154155f,//raw522:0.522
    0.99739062479973f,//raw523:0.523
    0.997158888750752f,//raw524:0.524
    0.996917324918077f,//raw525:0.525
    0.996665922167226f,//raw526:0.526
    0.996404682979379f,//raw527:0.527
    0.996133609932794f,//raw528:0.528
    0.995852688666413f,//raw529:0.529
    0.995561955439577f,//raw530:0.53
    0.99526139667117f,//raw531:0.531
    0.994951015327509f,//raw532:0.532
    0.994630795093547f,//raw533:0.5330001
    0.994300777301058f,//raw534:0.534
    0.993960946413997f,//raw535:0.535
    0.99361130578627f,//raw536:0.536
    0.993251858868602f,//raw537:0.537
    0.992882586907124f,//raw538:0.538
    0.992503537564863f,//raw539:0.539
    0.992114692865604f,//raw540:0.54
    0.991716056646987f,//raw541:0.541
    0.991307608207387f,//raw542:0.5420001
    0.990889400266454f,//raw543:0.543
    0.990461412898996f,//raw544:0.544
    0.990023650328966f,//raw545:0.545
    0.989576116876793f,//raw546:0.546
    0.989118789410793f,//raw547:0.5470001
    0.988651726959577f,//raw548:0.548
    0.988174907166199f,//raw549:0.549
    0.987688334736556f,//raw550:0.55
    0.9871920144728f,//raw551:0.551
    0.986685920818894f,//raw552:0.552
    0.986170119097845f,//raw553:0.553
    0.985644584526472f,//raw554:0.554
    0.985109322291454f,//raw555:0.555
    0.984564304901782f,//raw556:0.5560001
    0.984009602704464f,//raw557:0.557
    0.983445188979697f,//raw558:0.558
    0.982871069297874f,//raw559:0.559
    0.982287249325176f,//raw560:0.56
    0.981693699158024f,//raw561:0.561
    0.981090495407694f,//raw562:0.562
    0.980477608939585f,//raw563:0.563
    0.979855045802481f,//raw564:0.564
    0.979222812140671f,//raw565:0.565
    0.978580875645381f,//raw566:0.566
    0.977929319173256f,//raw567:0.567
    0.97726811118208f,//raw568:0.568
    0.976597258197541f,//raw569:0.569
    0.975916725992416f,//raw570:0.5700001
    0.975226602405013f,//raw571:0.571
    0.974526853972595f,//raw572:0.572
    0.97381748760122f,//raw573:0.573
    0.973098510291867f,//raw574:0.574
    0.972369885426894f,//raw575:0.575
    0.971631707052071f,//raw576:0.576
    0.970883939311487f,//raw577:0.577
    0.970126589585117f,//raw578:0.578
    0.969359619349455f,//raw579:0.5790001
    0.968583127599623f,//raw580:0.58
    0.967797076571502f,//raw581:0.581
    0.9670014740229f,//raw582:0.582
    0.96619632780589f,//raw583:0.583
    0.965381597023235f,//raw584:0.5840001
    0.964557386834651f,//raw585:0.585
    0.963723657099193f,//raw586:0.586
    0.962880416045222f,//raw587:0.587
    0.962027671994976f,//raw588:0.588
    0.961165381687573f,//raw589:0.589
    0.960293656421381f,//raw590:0.59
    0.959412453688528f,//raw591:0.591
    0.958521782185906f,//raw592:0.592
    0.957621596769374f,//raw593:0.5930001
    0.956712013628531f,//raw594:0.594
    0.955792988369486f,//raw595:0.595
    0.954864530062413f,//raw596:0.596
    0.953926647870584f,//raw597:0.597
    0.95297929430585f,//raw598:0.598
    0.952022591645943f,//raw599:0.599
    0.951056493149344f,//raw600:0.6
    0.950081008350808f,//raw601:0.601
    0.949096087895381f,//raw602:0.6020001
    0.948101858909665f,//raw603:0.603
    0.947098272782309f,//raw604:0.604
    0.946085339418048f,//raw605:0.605
    0.945063068813868f,//raw606:0.606
    0.944031409292309f,//raw607:0.6070001
    0.942990494012722f,//raw608:0.608
    0.941940272037305f,//raw609:0.609
    0.940880753731057f,//raw610:0.61
    0.939811949550727f,//raw611:0.611
    0.9387338055091f,//raw612:0.612
    0.937646460765423f,//raw613:0.613
    0.936549862068008f,//raw614:0.614
    0.935444020239562f,//raw615:0.615
    0.934328879454626f,//raw616:0.6160001
    0.933204583647725f,//raw617:0.617
    0.932071077725491f,//raw618:0.618
    0.930928372874882f,//raw619:0.619
    0.929776480373642f,//raw620:0.62
    0.928615342110943f,//raw621:0.6210001
    0.927445107958341f,//raw622:0.622
    0.926265720532642f,//raw623:0.623
    0.925077191473624f,//raw624:0.624
    0.923879532511287f,//raw625:0.625
    0.92267268326377f,//raw626:0.626
    0.921456799502683f,//raw627:0.627
    0.920231821569163f,//raw628:0.628
    0.918997761552938f,//raw629:0.629
    0.917754557266017f,//raw630:0.6300001
    0.916502369172459f,//raw631:0.631
    0.915241135803439f,//raw632:0.632
    0.913970869606498f,//raw633:0.633
    0.912691583118328f,//raw634:0.634
    0.911403211907126f,//raw635:0.6350001
    0.91010592226678f,//raw636:0.636
    0.908799650479694f,//raw637:0.637
    0.907484409437911f,//raw638:0.638
    0.906160132925989f,//raw639:0.6390001
    0.904826991872222f,//raw640:0.64
    0.903484920771285f,//raw641:0.641
    0.902133932868536f,//raw642:0.642
    0.900774041497338f,//raw643:0.643
    0.899405178227447f,//raw644:0.6440001
    0.898027519742103f,//raw645:0.645
    0.896640998315904f,//raw646:0.646
    0.895245627632907f,//raw647:0.647
    0.893841421464501f,//raw648:0.648
    0.892428309182526f,//raw649:0.649
    0.891006473181572f,//raw650:0.65
    0.889575843532876f,//raw651:0.651
    0.888136434355809f,//raw652:0.652
    0.886688173276396f,//raw653:0.6530001
    0.885231247225983f,//raw654:0.654
    0.883765584525508f,//raw655:0.655
    0.882291199640098f,//raw656:0.656
    0.880808107120962f,//raw657:0.657
    0.879316232427959f,//raw658:0.6580001
    0.877815768121775f,//raw659:0.659
    0.87630664035143f,//raw660:0.66
    0.874788864011023f,//raw661:0.661
    0.873262362840692f,//raw662:0.6620001
    0.871727333870468f,//raw663:0.663
    0.870183701524923f,//raw664:0.664
    0.868631481038694f,//raw665:0.665
    0.867070687731174f,//raw666:0.666
    0.86550124320983f,//raw667:0.6670001
    0.863923350047515f,//raw668:0.668
    0.862336930530066f,//raw669:0.669
    0.8607420003144f,//raw670:0.67
    0.859138575141429f,//raw671:0.671
    0.857526574505284f,//raw672:0.6720001
    0.855906206471663f,//raw673:0.673
    0.854277391206841f,//raw674:0.674
    0.852640144786153f,//raw675:0.675
    0.850994385027335f,//raw676:0.6760001
    0.849340324353476f,//raw677:0.677
    0.847677881249353f,//raw678:0.678
    0.846007072122185f,//raw679:0.679
    0.844327913461761f,//raw680:0.68
    0.842640321008608f,//raw681:0.6810001
    0.840944512585285f,//raw682:0.682
    0.839240404592837f,//raw683:0.683
    0.83752801384969f,//raw684:0.684
    0.835807254449544f,//raw685:0.6850001
    0.834078348495916f,//raw686:0.686
    0.832341210737685f,//raw687:0.687
    0.830595858319259f,//raw688:0.688
    0.828842308466118f,//raw689:0.689
    0.827080473232544f,//raw690:0.6900001
    0.825310580023832f,//raw691:0.691
    0.823532541542624f,//raw692:0.692
    0.821746375336992f,//raw693:0.693
    0.819952099035219f,//raw694:0.694
    0.818149622673869f,//raw695:0.6950001
    0.816339178903905f,//raw696:0.696
    0.814520678403274f,//raw697:0.697
    0.812694139119378f,//raw698:0.698
    0.810859469490557f,//raw699:0.6990001
    0.809016906323055f,//raw700:0.7
    0.807166358690899f,//raw701:0.701
    0.805307844857776f,//raw702:0.702
    0.803441383165995f,//raw703:0.703
    0.801566880076483f,//raw704:0.7040001
    0.799684577536909f,//raw705:0.705
    0.797794382636627f,//raw706:0.706
    0.795896314030616f,//raw707:0.707
    0.793990390451565f,//raw708:0.708
    0.79207651640607f,//raw709:0.7090001
    0.790154938923525f,//raw710:0.71
    0.788225563131535f,//raw711:0.711
    0.786288408071771f,//raw712:0.712
    0.784343376704298f,//raw713:0.7130001
    0.78239072008008f,//raw714:0.714
    0.78043034177415f,//raw715:0.715
    0.778462261134152f,//raw716:0.716
    0.776486497583746f,//raw717:0.717
    0.774502952171424f,//raw718:0.7180001
    0.772511880919268f,//raw719:0.719
    0.770513185483043f,//raw720:0.72
    0.768506885588559f,//raw721:0.721
    0.766492880772594f,//raw722:0.7220001
    0.764471430988718f,//raw723:0.723
    0.762442436374725f,//raw724:0.724
    0.760405916955457f,//raw725:0.725
    0.758361892830021f,//raw726:0.726
    0.7563102616679f,//raw727:0.7270001
    0.754251288279208f,//raw728:0.728
    0.752184870926484f,//raw729:0.729
    0.750111030003908f,//raw730:0.73
    0.748029785978925f,//raw731:0.731
    0.745941034678972f,//raw732:0.7320001
    0.74384504570537f,//raw733:0.733
    0.741741715470503f,//raw734:0.734
    0.739631064732859f,//raw735:0.735
    0.737512987864716f,//raw736:0.7360001
    0.735387758252533f,//raw737:0.737
    0.733255270846949f,//raw738:0.738
    0.731115546694213f,//raw739:0.739
    0.728968606911995f,//raw740:0.74
    0.726814344077115f,//raw741:0.7410001
    0.724653036246661f,//raw742:0.742
    0.722484576567451f,//raw743:0.743
    0.720308986440756f,//raw744:0.744
    0.718126157026088f,//raw745:0.7450001
    0.715936370067708f,//raw746:0.7460001
    0.713739517288333f,//raw747:0.747
    0.711535620369455f,//raw748:0.748
    0.709324701062087f,//raw749:0.749
    0.707106648778306f,//raw750:0.7500001
    0.704881749808692f,//raw751:0.751
    0.702649894119914f,//raw752:0.752
    0.700411103738917f,//raw753:0.753
    0.698165400761092f,//raw754:0.754
    0.695912672878365f,//raw755:0.7550001
    0.693653210857f,//raw756:0.756
    0.691386902934763f,//raw757:0.757
    0.689113771478622f,//raw758:0.758
    0.686833702824313f,//raw759:0.7590001
    0.684546991267053f,//raw760:0.7600001
    0.682253523681318f,//raw761:0.761
    0.679953322702124f,//raw762:0.762
    0.677646411030941f,//raw763:0.763
    0.675332673333537f,//raw764:0.7640001
    0.67301240825089f,//raw765:0.765
    0.670685500978515f,//raw766:0.766
    0.668351974481454f,//raw767:0.767
    0.66601185179008f,//raw768:0.768
    0.663665015928646f,//raw769:0.7690001
    0.661311769810212f,//raw770:0.77
    0.658951996979652f,//raw771:0.771
    0.656585720726373f,//raw772:0.772
    0.654212822782176f,//raw773:0.7730001
    0.651833609424029f,//raw774:0.774
    0.649447962896966f,//raw775:0.775
    0.647055906745748f,//raw776:0.776
    0.644657464578394f,//raw777:0.777
    0.642252516537434f,//raw778:0.7780001
    0.639841373036635f,//raw779:0.779
    0.637423914722404f,//raw780:0.78
    0.635000165453463f,//raw781:0.781
    0.632570004122191f,//raw782:0.7820001
    0.630133744396711f,//raw783:0.7830001
    0.627691265665706f,//raw784:0.784
    0.625242592034835f,//raw785:0.785
    0.622787747670896f,//raw786:0.786
    0.620326609930501f,//raw787:0.7870001
    0.617859496479992f,//raw788:0.788
    0.615386285162711f,//raw789:0.789
    0.612907000387626f,//raw790:0.79
    0.610421666623648f,//raw791:0.791
    0.607930159721883f,//raw792:0.7920001
    0.605432801268512f,//raw793:0.793
    0.602929467591675f,//raw794:0.794
    0.60042018339763f,//raw795:0.795
    0.597904823355113f,//raw796:0.7960001
    0.5953837121291f,//raw797:0.7970001
    0.592856724857521f,//raw798:0.798
    0.590323886480077f,//raw799:0.799
    0.587785221994218f,//raw800:0.8
    0.585240604618575f,//raw801:0.8010001
    0.582690362794454f,//raw802:0.802
    0.580134370199775f,//raw803:0.803
    0.577572652060502f,//raw804:0.804
    0.575005080457687f,//raw805:0.8050001
    0.572431986795405f,//raw806:0.8060001
    0.569853243605986f,//raw807:0.807
    0.56726887633993f,//raw808:0.808
    0.564678910503242f,//raw809:0.809
    0.562083216783426f,//raw810:0.8100001
    0.559482130214362f,//raw811:0.811
    0.556875521924728f,//raw812:0.812
    0.554263417640032f,//raw813:0.813
    0.551645843140025f,//raw814:0.814
    0.549022667750572f,//raw815:0.8150001
    0.546394230052693f,//raw816:0.816
    0.543760399803207f,//raw817:0.817
    0.541121202996288f,//raw818:0.818
    0.538476507891682f,//raw819:0.8190001
    0.535826655848023f,//raw820:0.8200001
    0.533171515547763f,//raw821:0.821
    0.530511113195391f,//raw822:0.822
    0.527845475047328f,//raw823:0.823
    0.525174468059952f,//raw824:0.8240001
    0.522498436988049f,//raw825:0.825
    0.519817249200299f,//raw826:0.826
    0.51713093115826f,//raw827:0.827
    0.514439348799233f,//raw828:0.8280001
    0.511742849533729f,//raw829:0.8290001
    0.509041299702949f,//raw830:0.83
    0.506334725969413f,//raw831:0.831
    0.503623155045224f,//raw832:0.832
    0.500906451623626f,//raw833:0.8330001
    0.498184966357582f,//raw834:0.8340001
    0.49545856433365f,//raw835:0.835
    0.492727272459625f,//raw836:0.836
    0.48999111769156f,//raw837:0.837
    0.487249963512032f,//raw838:0.8380001
    0.484504163729934f,//raw839:0.839
    0.48175358221047f,//raw840:0.84
    0.47899824610007f,//raw841:0.841
    0.47623801793701f,//raw842:0.8420001
    0.473473253992119f,//raw843:0.8430001
    0.470703817177685f,//raw844:0.844
    0.467929734826226f,//raw845:0.845
    0.465151034316112f,//raw846:0.846
    0.462367577035787f,//raw847:0.8470001
    0.459579722254332f,//raw848:0.848
    0.456787331723352f,//raw849:0.849
    0.453990433001905f,//raw850:0.85
    0.451189053693541f,//raw851:0.851
    0.448383054071076f,//raw852:0.8520001
    0.445572796313196f,//raw853:0.853
    0.442758141044936f,//raw854:0.854
    0.439939116045092f,//raw855:0.855
    0.437115580718783f,//raw856:0.8560001
    0.43428789950808f,//raw857:0.8570001
    0.431455932161505f,//raw858:0.858
    0.428619706628712f,//raw859:0.859
    0.425779250901382f,//raw860:0.86
    0.422934423331252f,//raw861:0.8610001
    0.420085591108646f,//raw862:0.862
    0.41723261291761f,//raw863:0.863
    0.41437551691516f,//raw864:0.864
    0.411514160635478f,//raw865:0.8650001
    0.408648913402299f,//raw866:0.8660001
    0.405779633073174f,//raw867:0.867
    0.402906347966013f,//raw868:0.868
    0.400029086438248f,//raw869:0.869
    0.397147705033772f,//raw870:0.8700001
    0.39426257566102f,//raw871:0.8710001
    0.391373555176012f,//raw872:0.872
    0.388480672091479f,//raw873:0.873
    0.385583954958274f,//raw874:0.874
    0.382683259365394f,//raw875:0.8750001
    0.379778959714214f,//raw876:0.876
    0.376870911894539f,//raw877:0.877
    0.373959144606888f,//raw878:0.878
    0.371043512701982f,//raw879:0.8790001
    0.368124392509071f,//raw880:0.8800001
    0.365201639170577f,//raw881:0.881
    0.362275281532152f,//raw882:0.882
    0.35934534847502f,//raw883:0.883
    0.356411693959309f,//raw884:0.8840001
    0.3534746966405f,//raw885:0.8850001
    0.350534210758968f,//raw886:0.886
    0.347590265335375f,//raw887:0.887
    0.344642713643302f,//raw888:0.8880001
    0.341691936131981f,//raw889:0.8890001
    0.338737786346033f,//raw890:0.89
    0.335780293440972f,//raw891:0.891
    0.332819486605306f,//raw892:0.892
    0.329855218287037f,//raw893:0.8930001
    0.32688787109306f,//raw894:0.8940001
    0.323917297730837f,//raw895:0.895
    0.320943527517975f,//raw896:0.896
    0.317966589803628f,//raw897:0.897
    0.31498633624662f,//raw898:0.8980001
    0.312003151517108f,//raw899:0.899
    0.309016887521741f,//raw900:0.9
    0.30602757373298f,//raw901:0.901
    0.30303506120465f,//raw902:0.9020001
    0.300039736189208f,//raw903:0.9030001
    0.29704144997895f,//raw904:0.904
    0.29404023216499f,//raw905:0.905
    0.291036112367373f,//raw906:0.906
    0.288028940916759f,//raw907:0.9070001
    0.28501910595768f,//raw908:0.9080001
    0.282006458047525f,//raw909:0.909
    0.278991026919147f,//raw910:0.91
    0.275972662351257f,//raw911:0.9110001
    0.272951753933114f,//raw912:0.9120001
    0.269928151660721f,//raw913:0.913
    0.266901885375042f,//raw914:0.914
    0.263872984943337f,//raw915:0.915
    0.260841299487748f,//raw916:0.9160001
    0.257807220316901f,//raw917:0.9170001
    0.254770596758388f,//raw918:0.918
    0.251731458781686f,//raw919:0.919
    0.248689836381086f,//raw920:0.92
    0.245645578059394f,//raw921:0.9210001
    0.242599076748051f,//raw922:0.9220001
    0.239550181143415f,//raw923:0.923
    0.236498921336078f,//raw924:0.924
    0.233445145360278f,//raw925:0.9250001
    0.230389247375929f,//raw926:0.9260001
    0.227331075601269f,//raw927:0.927
    0.224270660218443f,//raw928:0.928
    0.221208031431737f,//raw929:0.929
    0.218143036723433f,//raw930:0.9300001
    0.215076071701483f,//raw931:0.9310001
    0.212006984020192f,//raw932:0.932
    0.208935803969438f,//raw933:0.933
    0.205862561859746f,//raw934:0.934
    0.20278710465908f,//raw935:0.9350001
    0.199709829325812f,//raw936:0.936
    0.196630582987907f,//raw937:0.937
    0.193549396035502f,//raw938:0.938
    0.190466115052279f,//raw939:0.9390001
    0.187381138006452f,//raw940:0.9400001
    0.18429431163206f,//raw941:0.941
    0.181205666394051f,//raw942:0.942
    0.178115232775321f,//raw943:0.943
    0.17502285691328f,//raw944:0.9440001
    0.171928937950046f,//raw945:0.9450001
    0.168833322161298f,//raw946:0.946
    0.165736040098726f,//raw947:0.947
    0.162636937570054f,//raw948:0.9480001
    0.159536414585627f,//raw949:0.9490001
    0.156434317081739f,//raw950:0.95
    0.153330675674052f,//raw951:0.951
    0.150225520993464f,//raw952:0.952
    0.147118698469832f,//raw953:0.9530001
    0.144010609109947f,//raw954:0.9540001
    0.140901098460087f,//raw955:0.955
    0.137790197209077f,//raw956:0.956
    0.134677936059465f,//raw957:0.957
    0.131564160101382f,//raw958:0.9580001
    0.128449271239118f,//raw959:0.9590001
    0.125333114667047f,//raw960:0.96
    0.122215721139584f,//raw961:0.961
    0.119096935502592f,//raw962:0.9620001
    0.115977160306977f,//raw963:0.9630001
    0.112856240493086f,//raw964:0.964
    0.109734206862343f,//raw965:0.965
    0.106611090227167f,//raw966:0.966
    0.103486735162547f,//raw967:0.9670001
    0.10036154493825f,//raw968:0.9680001
    0.0972353642115271f,//raw969:0.969
    0.0941082238357255f,//raw970:0.97
    0.0909799681967431f,//raw971:0.9710001
    0.0878510010678056f,//raw972:0.9720001
    0.0847211669072807f,//raw973:0.973
    0.0815904966045717f,//raw974:0.974
    0.0784590210573354f,//raw975:0.975
    0.075326584449665f,//raw976:0.9760001
    0.072193591094437f,//raw977:0.9770001
    0.0690598852359568f,//raw978:0.978
    0.0659254978018404f,//raw979:0.979
    0.0627904597264296f,//raw980:0.98
    0.059654615030459f,//raw981:0.9810001
    0.0565183684666955f,//raw982:0.9820001
    0.0533815641038158f,//raw983:0.983
    0.0502442329000154f,//raw984:0.984
    0.0471062187730489f,//raw985:0.9850001
    0.0439679267556984f,//raw986:0.9860001
    0.0408292008038349f,//raw987:0.987
    0.037690071894619f,//raw988:0.988
    0.0345505710091879f,//raw989:0.989
    0.0314105419712335f,//raw990:0.9900001
    0.0282703900736083f,//raw991:0.9910001
    0.0251299591658307f,//raw992:0.992
    0.0219892802418878f,//raw993:0.993
    0.0188483842982146f,//raw994:0.994
    0.0157071151029744f,//raw995:0.9950001
    0.0125658781090905f,//raw996:0.9960001
    0.00942451709825605f,//raw997:0.997
    0.00628306307363777f,//raw998:0.998
    0.0031413597867302f,//raw999:0.9990001
 };


    #endregion

}



[System.Serializable]
public class VectorXZ
{
    public float x, z;
}



[System.Serializable]
public class SeaMaterial
{
    public Vector2 move;
    public Material material;
}


