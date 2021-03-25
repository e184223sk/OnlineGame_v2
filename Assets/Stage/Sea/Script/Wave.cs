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
                    if (wave.transform.position.y - 1 > obj.transform.position.y)
                    {
                        if (obj.GetComponent<MeshRenderer>() != null)
                        {
                            if (obj.GetComponent<NonCastSeaShadowObject>() == null)
                            {
                                if (obj.GetComponent<CastSeaShadowObject>() == null)
                                {
                                    obj.gameObject.AddComponent<CastSeaShadowObject>();
                                }
                            } 
                        }
                    }
                    else if (obj.GetComponent<CastSeaShadowObject>() != null)
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
       

        
        for (int x = 0; x < VerticesToVector - 1; x++)
          {
              for (int z = 0; z < VerticesToVector - 1; z++)
              {
                
                   int i = z * VerticesToVector + x;
                   var v = vertexList[i];
                   
                  float d = ((float)(x - VerticesToVector / 2) * (x - VerticesToVector / 2) + (float)(z - VerticesToVector / 2) * (z - VerticesToVector / 2)) ;
                  float sinx = sinArray[(int)((WaveInterval * x * Cnt)%1*1000)] ; 
                  float sinz = sinArray[(int)((WaveInterval * z * Cnt) % 1 * 1000)];

                //ここで波を加工 -----------------------------
                v.y = WaveHeight * (d / VerticesToVector + 1) * 
                  (
                      sinx* sinx * Waveheight_PlaneAxis.x + sinz * sinz * Waveheight_PlaneAxis.z +
                      sinArray[(int)(WaveInterval * (d + 1) * WaveRing * sinArray[(int)(Cnt % 1 * 1000)]   ) % 1 * 1000] * WaveRingHeight
                  );
                  //-------------------------------------------- 
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


    #region さいんのはいれつ
    float[] sinArray = new float[1000]
{
    0f,
    0.0009999998808307693f,
    0.001999998761661646f,
    0.0029999955260789396f,
    0.003999989523330152f,
    0.004999979520590863f,
    0.005999964052217925f,
    0.006999943049534935f,
    0.007999915046907185f,
    0.008999879044362431f,
    0.009999834041936431f,
    0.010999779039673946f,
    0.01199971210637421f,
    0.012999634104625667f,
    0.013999543103239838f,
    0.014999438102307484f,
    0.015999318101933363f,
    0.016999182102237224f,
    0.017999029103354828f,
    0.01899885810543893f,
    0.01999866810866028f,
    0.02099845811320863f,
    0.021998227119293737f,
    0.022997974127146343f,
    0.02399769627491047f,
    0.02499739628712495f,
    0.025997071301937034f,
    0.02699672031967146f,
    0.027996342340678968f,
    0.028995936365337288f,
    0.029995501394052156f,
    0.03099503642725829f,
    0.031994540465420426f,
    0.03299401064740324f,
    0.03399345155862754f,
    0.034992854753256544f,
    0.03599222667802917f,
    0.03699155888769161f,
    0.03799085782852488f,
    0.038990115055891936f,
    0.03998933701559011f,
    0.040988515263632896f,
    0.04198765624530816f,
    0.0429867515173137f,
    0.043985807524402155f,
    0.044984815823989395f,
    0.045983782860267025f,
    0.04698270019140278f,
    0.04798157054000188f,
    0.04898039662801643f,
    0.04997917001480526f,
    0.050977897143044654f,
    0.05197656957292968f,
    0.05297519374648542f,
    0.05397376122477696f,
    0.054972278449152374f,
    0.05597073698158053f,
    0.05696914326270674f,
    0.05796748885543741f,
    0.05896578019968931f,
    0.05996400885934014f,
    0.06096218127355238f,
    0.061960289007208756f,
    0.0629583347807918f,
    0.0639563213139227f,
    0.06495424389047841f,
    0.06595209407817325f,
    0.06694988574835899f,
    0.06794761046839319f,
    0.0689452672405472f,
    0.06994284763482612f,
    0.0709403655188313f,
    0.07193781246218933f,
    0.07293518746744931f,
    0.07393248210704209f,
    0.0749297102445966f,
    0.07592686345214342f,
    0.07692394073252536f,
    0.07792093366073378f,
    0.07891785609620348f,
    0.0799146996135038f,
    0.08091145578963492f,
    0.08190813848074051f,
    0.08290473926338246f,
    0.08390125714095603f,
    0.08489768369325781f,
    0.08589403277184927f,
    0.086890295956067f,
    0.08788647224964381f,
    0.08888255323530746f,
    0.08987855275981559f,
    0.09087446240541415f,
    0.09187028117618955f,
    0.0928660006579353f,
    0.0938616346923837f,
    0.0948571748648248f,
    0.0958526127634398f,
    0.09684796222604665f,
    0.09784321484030681f,
    0.0988383696109637f,
    0.0998334181294999f,
    0.10082837422832049f,
    0.10182322949836657f,
    0.10281798294477895f,
    0.10381262616247605f,
    0.10480717297822881f,
    0.10580161498639251f,
    0.10679595119252122f,
    0.10779017319510377f,
    0.10878429481505562f,
    0.10977830765028082f,
    0.11077221070676257f,
    0.11176599558669449f,
    0.11275967610491554f,
    0.11375324386301286f,
    0.11474669046604682f,
    0.11574002972415394f,
    0.11673325324177188f,
    0.11772636002567316f,
    0.11871934168485808f,
    0.11971221202299985f,
    0.12070496264845436f,
    0.12169759256846704f,
    0.12269009339611235f,
    0.12368247892837932f,
    0.12467473338522769f,
    0.12566687056022163f,
    0.12665888206784615f,
    0.12765076691608576f,
    0.12864252411305174f,
    0.12963415266698292f,
    0.13062565158624678f,
    0.13161700510780988f,
    0.1326082417853299f,
    0.1335993458540817f,
    0.1345903163229573f,
    0.13558115220098224f,
    0.13657185249731676f,
    0.13756241622125664f,
    0.13855284238223423f,
    0.1395431152344512f,
    0.14053326330043917f,
    0.1415232708326059f,
    0.14251313684094002f,
    0.14350286033557155f,
    0.14449244032677305f,
    0.14548187582496064f,
    0.14647115110024356f,
    0.14746029464642044f,
    0.14844929073171728f,
    0.1494381383671341f,
    0.1504268365638193f,
    0.15141538433307078f,
    0.15240378068633684f,
    0.1533920246352172f,
    0.15438010046894496f,
    0.1553680366467715f,
    0.15635581745594462f,
    0.1573434419086796f,
    0.15833090901734806f,
    0.15931821779447894f,
    0.1603053672527596f,
    0.16129234169898118f,
    0.1622791695606723f,
    0.16326583514254964f,
    0.16425233745794368f,
    0.16523867552034824f,
    0.1662248483434213f,
    0.16721085494098617f,
    0.16819669432703227f,
    0.1691823508293589f,
    0.17016785283753422f,
    0.17115318467718116f,
    0.172138345362964f,
    0.1731233339097181f,
    0.17410814933245106f,
    0.17509279064634348f,
    0.1760772421983995f,
    0.17706153234348146f,
    0.17804564542632814f,
    0.17902958046282264f,
    0.18001333646902593f,
    0.18099691246117816f,
    0.1819803074556994f,
    0.18296352046919076f,
    0.18394653587154386f,
    0.18492938197625602f,
    0.18591204315085208f,
    0.18689451841266697f,
    0.1878768067792215f,
    0.1888589072682234f,
    0.18984081889756835f,
    0.19082252605799507f,
    0.19180405702532005f,
    0.1927853961878266f,
    0.19376654256417164f,
    0.19474749517320492f,
    0.1957282530339699f,
    0.19670881516570485f,
    0.19768918058784374f,
    0.19866933371588721f,
    0.19964930278089146f,
    0.20062907219580037f,
    0.2016086409808406f,
    0.20258800815643951f,
    0.20356717274322605f,
    0.2045461337620317f,
    0.20552489023389162f,
    0.20650342660006568f,
    0.20748177104504295f,
    0.2084599080074255f,
    0.2094378365090725f,
    0.21041555557205158f,
    0.2113930642186398f,
    0.21237036147132457f,
    0.21334743179472215f,
    0.2143243033310955f,
    0.21530096054231473f,
    0.21627740245171873f,
    0.21725362808286167f,
    0.2182296364595141f,
    0.21920542660566372f,
    0.22018099754551654f,
    0.22115633377131366f,
    0.22213146337537137f,
    0.2231063708470839f,
    0.22408105521153993f,
    0.22505551549405114f,
    0.22602975072015347f,
    0.22700375991560778f,
    0.22797752759764117f,
    0.22895108181339166f,
    0.22992440707715148f,
    0.23089750241559145f,
    0.23187036685561238f,
    0.232842999424346f,
    0.23381539914915586f,
    0.23478756505763837f,
    0.23575948169650493f,
    0.23673117705957855f,
    0.23770263569053537f,
    0.23867385661791285f,
    0.23964483887048624f,
    0.2406155814772694f,
    0.24158608346751592f,
    0.2425563294145473f,
    0.24352634726406613f,
    0.24449612158627096f,
    0.24546565141138357f,
    0.24643493576987036f,
    0.24740395925452294f,
    0.24837276421005997f,
    0.2493412919234086f,
    0.25030959915549716f,
    0.25127762722341657f,
    0.25224543285881335f,
    0.2532129574091769f,
    0.2541802287531241f,
    0.25514727473933996f,
    0.25611403676135136f,
    0.25708057147660773f,
    0.25804682030962084f,
    0.2590128398877644f,
    0.2599785716667695f,
    0.2609440434739188f,
    0.2619092831057633f,
    0.2628742320653015f,
    0.26383894690372767f,
    0.2648033691558446f,
    0.2657675553419787f,
    0.26673144702897106f,
    0.267695100706054f,
    0.26865845797234067f,
    0.2696215465871094f,
    0.27058439427788095f,
    0.27154694269261404f,
    0.27250924824181805f,
    0.2734712526063075f,
    0.2744330121647075f,
    0.27539446863092154f,
    0.2763556497097811f,
    0.27731658307356344f,
    0.2782772104862426f,
    0.2792375882457468f,
    0.28019765814972164f,
    0.2811574764634224f,
    0.282116985018399f,
    0.28307621146367834f,
    0.2840351834149386f,
    0.2849938427550224f,
    0.28595224566651833f,
    0.286910334066755f,
    0.2878681641048607f,
    0.2888256777328823f,
    0.2897829025426872f,
    0.2907398660920012f,
    0.2916965103853834f,
    0.2926528914873298f,
    0.2936089514376979f,
    0.29456474626673806f,
    0.29552021804983797f,
    0.29647542278277833f,
    0.29743030257670877f,
    0.29838488494801996f,
    0.29933919737794373f,
    0.3002931820317066f,
    0.3012468948179353f,
    0.302200277938197f,
    0.3031533872658653f,
    0.3041061650390791f,
    0.30505863871398364f,
    0.30601083571077625f,
    0.30696269832288625f,
    0.3079142823345774f,
    0.3088655300764282f,
    0.3098164972966677f,
    0.3107671263632542f,
    0.31171744467074214f,
    0.3126674795769508f,
    0.3136171735063399f,
    0.31456658211607685f,
    0.3155156478685773f,
    0.316464426384194f,
    0.31741286016352854f,
    0.3183609765382025f,
    0.3193088028023164f,
    0.3202562815141816f,
    0.32120346820114193f,
    0.3221503054602695f,
    0.32309684878131545f,
    0.32404304080034224f,
    0.32498893696928643f,
    0.3259344799634303f,
    0.3268796970315138f,
    0.3278246153837489f,
    0.32876917775467684f,
    0.3297134395007282f,
    0.33065734339623704f,
    0.3316009447590436f,
    0.3325441864035037f,
    0.3334870955123678f,
    0.33442969922907684f,
    0.3353719404284474f,
    0.336313874330877f,
    0.33725544385177536f,
    0.3381967041721758f,
    0.33913759824830975f,
    0.34007815319560614f,
    0.3410183960894051f,
    0.34195826994759754f,
    0.342897829851842f,
    0.3438370188614211f,
    0.34477589201785747f,
    0.3457143924220532f,
    0.34665254712078714f,
    0.3475903831199717f,
    0.3485278435833644f,
    0.3494649834511859f,
    0.3504017459293816f,
    0.3513381859172665f,
    0.3522742466632014f,
    0.3532099830253759f,
    0.354145338294793f,
    0.35508033942802025f,
    0.35601501333976204f,
    0.35694930338540976f,
    0.3578832643193805f,
    0.35881683954027466f,
    0.35975008376061673f,
    0.36068294042244253f,
    0.3616154364106452f,
    0.36254759856748203f,
    0.3634793704005658f,
    0.36441080651673285f,
    0.36534185046759665f,
    0.36627255681733556f,
    0.36720286916178957f,
    0.36813281431286016f,
    0.3690624190390422f,
    0.36999162700293803f,
    0.37092049266112737f,
    0.37184895972100274f,
    0.3727770825957229f,
    0.37370480503769576f,
    0.37463215378451725f,
    0.3755591555296082f,
    0.376485754093321f,
    0.37741200377931056f,
    0.37833784845350665f,
    0.37926334237538223f,
    0.38018842945666914f,
    0.38111316391244127f,
    0.3820374897004569f,
    0.38296143346085176f,
    0.3838850217886015f,
    0.3848081987109238f,
    0.38573101833094703f,
    0.38665342472247544f,
    0.3875754719434799f,
    0.38849710411457505f,
    0.38941834779860185f,
    0.3903392295124777f,
    0.39125969344745104f,
    0.3921797935476544f,
    0.3930994740517048f,
    0.3940187888578216f,
    0.39493768225221343f,
    0.39585618071912515f,
    0.3967743106961167f,
    0.39769201654120157f,
    0.39860935203687353f,
    0.3995262615892945f,
    0.40044279893429124f,
    0.4013589085263965f,
    0.402274644054555f,
    0.4031899500218925f,
    0.4041048528096953f,
    0.4050193787515893f,
    0.4059334724240054f,
    0.4068471873977447f,
    0.40776046829838586f,
    0.40867336864909704f,
    0.409585833124826f,
    0.4104978880253024f,
    0.4114095596018394f,
    0.41232079260385274f,
    0.4132316404344929f,
    0.41414204789309766f,
    0.41505206833443614f,
    0.41596164660798896f,
    0.41687080893064044f,
    0.4177795814701047f,
    0.4186879091514896f,
    0.4195958452076783f,
    0.42050333461447253f,
    0.4214104305556279f,
    0.42231707805786023f,
    0.42322330325392393f,
    0.42412913222665194f,
    0.42503451007954296f,
    0.4259394898726041f,
    0.42684401676079753f,
    0.4277481437542592f,
    0.428651816059634f,
    0.4295550866369747f,
    0.4304579007448282f,
    0.4313602844059007f,
    0.43226226359201464f,
    0.4331637836399797f,
    0.43406489738371495f,
    0.43496555021247907f,
    0.4358657949093673f,
    0.43676557691630563f,
    0.43766492216894975f,
    0.43856385655132585f,
    0.43946232558476966f,
    0.4403603819243945f,
    0.44125797114474846f,
    0.44215514584938315f,
    0.4430518516662771f,
    0.44394811444276444f,
    0.4448439599738068f,
    0.4457393339679355f,
    0.4466342888988778f,
    0.44752877052913853f,
    0.44842283128014765f,
    0.4493164169686006f,
    0.4502095533522434f,
    0.45110226613570925f,
    0.4519945012173852f,
    0.45288631088704145f,
    0.4537776410977969f,
    0.4546685440863915f,
    0.4555589658608924f,
    0.4564489586048001f,
    0.45733846838134606f,
    0.45822752083123774f,
    0.4591161415411214f,
    0.4600042766573786f,
    0.4608919782294994f,
    0.46177919245956833f,
    0.4626659713431067f,
    0.46355226113811737f,
    0.46443808739284154f,
    0.46532347560072457f,
    0.46620837210404936f,
    0.4670928287625058f,
    0.467976791974832f,
    0.46886031354602203f,
    0.46974333993148365f,
    0.4706258965857399f,
    0.47150800890778616f,
    0.47238962343843577f,
    0.47327079184503745f,
    0.47415146072560915f,
    0.4750316816920795f,
    0.4759114013998847f,
    0.4767906452085824f,
    0.4776694384214734f,
    0.47854772778052107f,
    0.479425538604203f,
    0.48030292229412525f,
    0.4811797733855365f,
    0.48205614330960445f,
    0.4829320311899817f,
    0.48380743615080307f,
    0.4846824094522862f,
    0.48555684591941745f,
    0.4864307968422459f,
    0.4873042613468431f,
    0.488177290579393f,
    0.4890497795985661f,
    0.48992177958059285f,
    0.49079328965349595f,
    0.49166430894578766f,
    0.49253488845995946f,
    0.49340492354914695f,
    0.4942744652461567f,
    0.4951435126814695f,
    0.4960121167417041f,
    0.49688017301745296f,
    0.49774773242586445f,
    0.4986147940994015f,
    0.4994813571710249f,
    0.5003474723813683f,
    0.5012130356201933f,
    0.5020780976589301f,
    0.5029426576325391f,
    0.5038067146764826f,
    0.5046703193841411f,
    0.5055333679470474f,
    0.5063959109896449f,
    0.5072579476494128f,
    0.5081195284010173f,
    0.5089805496792785f,
    0.5098410619901382f,
    0.5107010644731065f,
    0.5115605562682027f,
    0.5124195877005658f,
    0.5132780555114534f,
    0.5141360100575446f,
    0.5149934504809072f,
    0.5158504269861512f,
    0.5167068365615443f,
    0.5175627294434486f,
    0.5184181047759933f,
    0.5192729617038252f,
    0.520127350279765f,
    0.520981167803159f,
    0.5218344643588432f,
    0.5226872390935434f,
    0.5235394911545067f,
    0.5243912704415131f,
    0.525242474567555f,
    0.5260931534646907f,
    0.526943306282263f,
    0.5277929827967207f,
    0.5286420808738171f,
    0.5294906503224888f,
    0.5303386902941881f,
    0.531186199940897f,
    0.5320332288837807f,
    0.5328796753068402f,
    0.53372558886399f,
    0.5345709687093384f,
    0.5354158139975272f,
    0.5362601741931977f,
    0.5371039478011443f,
    0.5379471843190178f,
    0.5387898829036033f,
    0.5396320928934321f,
    0.540473713051761f,
    0.5413147927503384f,
    0.5421553311481068f,
    0.542995327404549f,
    0.5438348306994493f,
    0.5446737401214191f,
    0.5455121048837192f,
    0.5463499241480061f,
    0.5471872469661806f,
    0.5480239726889556f,
    0.5488601504019147f,
    0.5496957792689017f,
    0.5505308584543094f,
    0.551365436849081f,
    0.5521994141338193f,
    0.5530328392334082f,
    0.553865711314444f,
    0.554698029544076f,
    0.5555298426510681f,
    0.5563610506484209f,
    0.5571917022990952f,
    0.5580217967724607f,
    0.5588513826666598f,
    0.5596803602624124f,
    0.5605087781922626f,
    0.5613366356278139f,
    0.5621639317412301f,
    0.5629907149662844f,
    0.5638168859205877f,
    0.5646424930725696f,
    0.5654675355966446f,
    0.5662920617942176f,
    0.5671159725542027f,
    0.5679393162128651f,
    0.5687620919468824f,
    0.5695842989335004f,
    0.5704059853075779f,
    0.5712270522993863f,
    0.5720475480788986f,
    0.5728674718256401f,
    0.5736868227197084f,
    0.5745056487282131f,
    0.5753238514252521f,
    0.5761414788133016f,
    0.5769585300747554f,
    0.5777750530416598f,
    0.5785909495649466f,
    0.5794062675122303f,
    0.5802210060682139f,
    0.5810351644181799f,
    0.5818487902242699f,
    0.5826617856856652f,
    0.5834741985003263f,
    0.5842860278558614f,
    0.5850972729404622f,
    0.5859079812451738f,
    0.586718055319872f,
    0.5875275426916712f,
    0.5883364425511048f,
    0.5891448022514858f,
    0.5899525246250006f,
    0.5907596570612307f,
    0.5915661987530644f,
    0.5923721488939809f,
    0.5931775546640626f,
    0.5939823192505695f,
    0.5947864898701011f,
    0.5955900657185075f,
    0.5963930938364371f,
    0.5971954776969524f,
    0.5979972643774168f,
    0.5987984530760646f,
    0.5995990429917277f,
    0.6003990809897042f,
    0.6011984709024798f,
    0.601997259632315f,
    0.6027954463804415f,
    0.6035930303486932f,
    0.6043900582258463f,
    0.6051864342062144f,
    0.6059822050157816f,
    0.6067773698587977f,
    0.6075719752819805f,
    0.608365925770831f,
    0.6091592679094713f,
    0.6099520009045798f,
    0.6107441239634438f,
    0.6115356834541613f,
    0.612326584228366f,
    0.6131168726918044f,
    0.6139065480542083f,
    0.6146956565399446f,
    0.6154841032952669f,
    0.6162719345823853f,
    0.6170591496134888f,
    0.6178457476013829f,
    0.6186317745897313f,
    0.6194171360951949f,
    0.6202018781995233f,
    0.620986000117995f,
    0.621769501066508f,
    0.6225524269068722f,
    0.6233346835285177f,
    0.6241163168315821f,
    0.6248973260344519f,
    0.6256777568526277f,
    0.6264575154754504f,
    0.6272366476569405f,
    0.628015152617986f,
    0.628793029580102f,
    0.6295703240748742f,
    0.6303469426686414f,
    0.6311229309317492f,
    0.6318982880882296f,
    0.6326730595217163f,
    0.6334471521018291f,
    0.6342206112511534f,
    0.6349934361962498f,
    0.6357656261643134f,
    0.6365372263530179f,
    0.6373081440131785f,
    0.6380784243816582f,
    0.6388480666881967f,
    0.6396170701631714f,
    0.640385479817166f,
    0.6411532032845096f,
    0.6419202856152125f,
    0.6426867260422121f,
    0.6434525694256091f,
    0.6442177237082078f,
    0.6449822337897245f,
    0.6457460989056688f,
    0.6465093182921954f,
    0.6472719366202926f,
    0.6480338622204285f,
    0.6487951398034424f,
    0.6495557686080765f,
    0.6503157478737215f,
    0.6510751220811368f,
    0.651833799950746f,
    0.6525918260033935f,
    0.653349199481073f,
    0.6541059647115595f,
    0.6548620307288848f,
    0.6556174419010959f,
    0.6563721974728014f,
    0.657126296689265f,
    0.6578797836860347f,
    0.658632567891198f,
    0.6593846934808073f,
    0.6601361597027567f,
    0.660887010538017f,
    0.6616371557315522f,
    0.6623866393050235f,
    0.6631354605089663f,
    0.6638836185945791f,
    0.6646311573486229f,
    0.665377986914186f,
    0.6661241511189505f,
    0.6668696492167715f,
    0.6676144804621701f,
    0.6683586884466031f,
    0.6691021837135295f,
    0.6698450098955567f,
    0.6705871662498777f,
    0.6713286962109204f,
    0.6720695106440521f,
    0.6728096530250344f,
    0.6735491226137438f,
    0.67428791867073f,
    0.6750260844331584f,
    0.6757635311707852f,
    0.6765003021623376f,
    0.6772363966710634f,
    0.6779718577755389f,
    0.6787065970706306f,
    0.679440657676658f,
    0.680174038859579f,
    0.6809067398860318f,
    0.6816388036353881f,
    0.6823701421108886f,
    0.6831007982338743f,
    0.6838307712737078f,
    0.684560060500435f,
    0.6852887085931512f,
    0.6860166279656716f,
    0.6867438613392856f,
    0.6874704079867783f,
    0.688196310426255f,
    0.6889214814015685f,
    0.6896459634731972f,
    0.6903697559166775f,
    0.6910928580082358f,
    0.6918153120637837f,
    0.6925370312416823f,
    0.6932580579004397f,
    0.6939783913190476f,
    0.6946980307771913f,
    0.6954170183875309f,
    0.6961352677251073f,
    0.6968528209453988f,
    0.6975696773308706f,
    0.6982858788308232f,
    0.6990013393551979f,
    0.6997161008962902f,
    0.700430162739357f,
    0.7011435241703547f,
    0.701856226933441f,
    0.7025681853591182f,
    0.7032794412347593f,
    0.7039899938491264f,
    0.7046998847815107f,
    0.7054090287004073f,
    0.7061174672284976f,
    0.7068251996573613f,
    0.7075322252792844f,
    0.7082385854665403f,
    0.7089441953120326f,
    0.7096490962316433f,
    0.7103532875204897f,
    0.7110567684743987f,
    0.7117595802575918f,
    0.712461638389504f,
    0.7131629840781826f,
    0.7138636166222997f,
    0.7145635770188937f,
    0.7152627811301469f,
    0.7159612699970959f,
    0.7166590429212699f,
    0.7173560992049138f,
    0.7180524796351659f,
    0.7187481005045332f,
    0.7194430026443669f,
    0.7201371853597829f,
    0.7208306892692637f,
    0.7215234310110851f,
    0.722215451248114f,
    0.7229067492883483f,
    0.7235973244405077f,
    0.7242872171113427f,
    0.724976344373213f,
    0.7256647466774668f,
    0.7263524233357196f,
    0.7270393736603125f,
    0.7277256378452536f,
    0.7284111333990602f,
    0.72909590056055f,
    0.7297799386449734f,
    0.7304632876754207f,
    0.7311458655108213f,
    0.7318277122192436f,
    0.7325088271188586f,
    0.7331892095285688f,
    0.7338688992569173f,
    0.7345478146026952f,
    0.7352259954196336f,
    0.7359034410295693f,
    0.73658019106869f,
    0.7372561641891483f,
    0.7379314000724876f,
    0.7386058980434893f,
    0.739279657427673f,
    0.7399527176448917f,
    0.7406249977908282f,
    0.7412965373308992f,
    0.7419673355935823f,
    0.7426373919080964f,
    0.743306745476975f,
    0.7439753158414533f,
    0.7446431422498343f,
    0.745310224034309f,
    0.745976600222848f,
    0.7466421907145786f,
    0.7473070345834062f,
    0.7479711311645038f,
    0.7486344797937922f,
    0.7492971192801614f,
    0.7499589699719046f,
    0.7506200707240509f,
    0.7512804208755168f,
    0.7519400197659694f,
    0.7525989059842478f,
    0.7532570003298021f,
    0.7539143414378146f,
    0.7545709286509609f,
    0.755226800381345f,
    0.7558818777907691f,
    0.7565361993378416f,
    0.7571897643682578f,
    0.7578425722284697f,
    0.758494661108798f,
    0.7591459526257573f,
    0.7597964850163746f,
    0.7604462576301343f,
    0.7610953084792423f,
    0.761743559545396f,
    0.7623910488876676f,
    0.7630377758585849f,
    0.7636837398114373f,
    0.7643289785349333f,
    0.7649734144690019f,
    0.765617085449417f,
    0.7662599908325244f,
    0.7669021299754353f,
    0.7675435404424157f,
    0.768184145133565f,
    0.7688239816604133f,
    0.7694630493831404f,
    0.7701013856857817f,
    0.7707389138379623f,
    0.7713756712711392f,
    0.7720116573485711f,
    0.7726468714342887f,
    0.7732813506861984f,
    0.7739150188375572f,
    0.7745479130938892f,
    0.7751800328223164f,
    0.775811414999173f,
    0.7764419837299957f,
    0.7770717760388921f,
    0.7777007912960859f,
    0.7783290288725784f,
    0.7789565255169068f,
    0.779583205801663f,
    0.780209106523352f,
    0.7808342270560891f,
    0.7814585667747699f,
    0.7820821621992164f,
    0.7827049383709617f,
    0.7833269318579877f,
    0.7839481420383171f,
    0.7845686052481412f,
    0.7851882469054969f,
    0.785807103394889f,
    0.786425174097477f,
    0.7870424583952061f,
    0.7876589923939131f,
    0.7882747019839406f,
    0.788889623319629f,
    0.789503755786073f,
    0.7901171353041749f,
    0.7907296881434575f,
    0.7913414502734782f,
    0.791952421082491f,
    0.7925625999595406f,
    0.7931720225935542f,
    0.7937806157296867f,
    0.7943884151057086f,
    0.7949954201138363f,
    0.7956016301470806f,
    0.796207080661501f,
    0.7968116988797156f,
    0.797415520306815f,
    0.7980185443389931f,
    0.7986208062453767f,
    0.7992222336318662f,
    0.799822861816768f,
    0.8004226901994692f,
    0.801021718180157f,
    0.8016199807934994f,
    0.8022174061261265f,
    0.8028140292620706f,
    0.8034098496047241f,
    0.8040048665582818f,
    0.8045991149220773f,
    0.8051925232652682f,
    0.8057851264367358f,
    0.8063769238438924f,
    0.8069679500971761f,
    0.8075581341530544f,
    0.8081475106716598f,
    0.8087360790636308f,
    0.8093238387404142f,
    0.8099108240755523f,
    0.8104969645112453f,
    0.8110822944709108f,
    0.8116668133692341f,
    0.8122505553896204f,
    0.8128334503641287f,
    0.8134155325261839f,
    0.8139968012937192f,
    0.8145772560854806f,
    0.8151569308464346f,
    0.8157357558975398f,
    0.8163137652339614f,
    0.8168909582777051f,
    0.8174673344515927f,
    0.8180429274613032f,
    0.8186176681184367f,
    0.8191915901790489f,
    0.8197646930692326f,
    0.8203370103026291f,
    0.8209084730845989f,
    0.8214791149793016f,
    0.8220489354161101f,
    0.8226179338252186f,
    0.8231861434794686f,
    0.8237534960779668f,
    0.8243200249442483f,
    0.8248857295117991f,
    0.8254506428602206f,
    0.8260146970848479f,
    0.826577925316116f,
    0.8271403269908112f,
    0.8277019015465464f,
    0.8282626818206305f,
    0.8288226004052088f,
    0.8293816901885973f,
    0.8299399506117204f,
    0.8304973811163322f,
    0.8310540142966278f,
    0.8316097832432486f,
    0.8321647206015687f,
    0.8327188258166648f,
    0.8332721312876537f,
    0.8338245705051794f,
    0.8343761759196744f,
    0.8349269469795473f,
    0.8354768831340414f,
    0.836026016537697f,
    0.8365742811826563f,
    0.8371217092749451f,
    0.8376683002671496f,
    0.8382140536126925f,
    0.8387590012207347f,
    0.839303077586566f,
    0.839846314671001f,
    0.8403887119308164f,
    0.8409303010782936f,
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


