using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Item;
/// <summary>
/// 店のアイテムを管理するクラス
/// </summary>
public class Shop : MonoBehaviour
{
    #region Public Properties

    //アイテムが自動で生成される間隔　〇秒
    [Range(0, 180)]
    public float _SpawnInterval;


    #endregion

    #region Private Properties

    //初期のアイテム生成個数
    [SerializeField, Range(2, 10)]
    private int _startItemNum = 2;

    //アイテム最大出現個数
    private int _maxItemNum = 10;


    //この店の在庫
    private List<ItemSuper> _stock = new List<ItemSuper>();

    //商品を配置する範囲
    [SerializeField]
    private Vector3 _productArea;


    //商品を配置する範囲の中心
    [SerializeField]
    private Vector3 _productCenter;

    //店の座標
    private Vector3 _position;

    //店のトランスフォーム
    private Transform _transform;

    //アイテムの親オブジェクト　（空）
    [SerializeField]
    private GameObject _itemParent;

    //経過時間
    private float _time;

    //最低生成個数
    [SerializeField, Range(0, 50)]
    private int _MinSpawn;

    //最大生成個数
    [SerializeField, Range(50, 100)]
    private int _MaxSpawn;


    private bool _IsStart =false;

    #endregion


    #region Unity Callbacks ----------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        //------------------------------------- 初期化 ---------------------------------------
        //在庫の初期化

        

        //アイテムの親オブジェクト　初期化
        _itemParent = new GameObject();
        _itemParent.name = "ItemParent" + this.gameObject.name;
        _itemParent.transform.parent = this.transform;
        _itemParent.transform.position = transform.TransformPoint(Vector3.zero);

        //position　代入
        _position = this.transform.position;
        //-------------------------------------　初期化終わり ------------------------------

        int[] a = new int[1];
        //------------------------------------ 生成 ----------------------------------------

        for(int i = 0; i < _startItemNum; i++)
        {
            GenerateItem();
        }

        //------------------------------------ 生成終わり ----------------------------------------

    }

    // Update is called once per frame
    void Update()
    {

        SpawnItem();
    }

    private void OnDrawGizmos()
    {

        //商品を生成する中心点を可視化
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_productCenter, 0.3f);

        //商品を生成するエリアを可視化
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_productCenter + transform.position, _productArea);
    }

    //入店処理
    private void OnTriggerEnter(Collider other)
    {
        PlayerStatus player = other.gameObject.GetComponent<PlayerStatus>();

        //入ってきたのがプレイヤーなら
        if (player != null)
        {
            player.EnterShop(this.gameObject);
        }
    }

    //退店処理
    private void OnTriggerExit(Collider other)
    {
        PlayerStatus player = other.gameObject.GetComponent<PlayerStatus>();

        //入ってきたのがプレイヤーなら
        if (player != null)
        {
            player.LeaveShop();
        }

        near_dis = 10000;
    }



    #endregion ----------------------------------------------------------------------------

    #region Public Methods ----------------------------------------------------------------------------

    private float near_dis = 10000;
    /// <summary>
    /// あるオブジェクトに一番近いアイテムの位置を返す
    /// </summary>
    /// <param name="objPos">対象のオブジェクト</param>
    /// <returns>一番近い</returns>
    public ItemSuper NearestItem(Vector3 objPos)
    {
        ItemSuper nearestItem = ItemSuper.Null;


        foreach (var i in _stock)
        {
            if (i == ItemSuper.Null) continue;

            
            float tmp_dis = Vector3.Distance(  i._object.transform.position, objPos);
            Debug.Log(i._object.name);
            Debug.Log(i._object.transform.position);
            Debug.Log(i.GetName() + " : " + tmp_dis.ToString());
            //より近いアイテムがあったら
            if (near_dis > tmp_dis)
            {
                nearestItem = i;

                near_dis = tmp_dis;
            }
            
        }
        return nearestItem;

    }


    public List<ItemSuper> GetStock()
    {
        return _stock;
    }

    #endregion  ----------------------------------------------------------------------------

    #region Private Methods ----------------------------------------------------------------------------


    private int count = 1;

    /// <summary>
    /// Updateで実行　一定間隔でランダムにアイテムを生成
    /// </summary>
    private void SpawnItem()
    {
        _time += Time.deltaTime;

        //最大出現個数を上回ったら生成しない
        if (_stock.Count >= _maxItemNum) _time = 0f;

        if (_time > _SpawnInterval)
        {
            //あらかじめ設定した値を追加するように
            GenerateItem();

            //これだと追加した後の変更が利かない　_objectの設定がうまくいってないと思われる
            /*_stock.AddItem(ItemSuper.RandomItemName());
            GameObject tmp_obj = _stock._ItemList[_stock._ItemList.Count - 1].Init(RandomPos());
            tmp_obj.transform.parent = _itemParent.transform;
            //tmp_obj.tra
            */
            _time = 0;
        }

    }


    private void GenerateItem()
    {
        ItemSuper item = ItemSuper.RandomItem();
        
        Vector3 pos = RandomPos();

        item._object = item.Init();
        item._object.transform.parent = _itemParent.transform;
        item._object.transform.position = pos;
        _stock.Add(item);


    }


    /// <summary>
    /// 商品を生成する位置をランダムで生成   （ローカル）
    /// </summary>
    /// <returns></returns>
    private Vector3 RandomPos()
    {
        float x, y, z;
        x = Random.Range(-_productArea.x / 2, _productArea.x / 2) + _productCenter.x;
        y = Random.Range(0, _productArea.y) + _productCenter.y;
        z = Random.Range(-_productArea.z / 2, _productArea.z / 2) + _productCenter.z;



        return transform.TransformPoint( new Vector3(x, y, z));
    }

    #endregion  ----------------------------------------------------------------------------
}
