﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Item;
/// <summary>
/// 店のアイテムを管理するクラス
/// </summary>
public class Shop : MonoBehaviour
{

    #region Shop Properties 

    //----------------------- 商品を配置する範囲 ----------------------------
    [SerializeField]
    private Vector3 _productArea;

    //----------------------- 商品を配置する範囲の中心 ---------------------
    [SerializeField]
    private Vector3 _productCenter;

    //----------------------- アイテムの親オブジェクト　（空） ---------------------
    [SerializeField]
    private GameObject _itemParent;

    //-----------------------プレイヤーが商品を買う店なのか、売る店なのか    true = 買う店、false = 売る店
    public bool _IsSalesShop;

    //----------------------- 経過時間 ---------------------------------------
    private float _time;

    //------------------------ スポーン間隔　〇秒 -----------------------------------------------
    [Range(0, 180)]
    public float _SpawnInterval;

    // ----------------------- スポーン間隔保持用変数 -----------------------------------------
    private float _saveSpawnInterval;

    //------------------------- 最近接アイテム距離保存用変数 -------------------------------
    private float _nearDis = 10000;

    #endregion  
    #region Item Properties 

    //-------------------------- 初期のアイテム生成個数 ---------------------------------
    [SerializeField, Range(2, 10)]
    private int _startItemNum = 2;

    //--------------------------- この店の在庫 -----------------------------------------
    private List<ItemSuper> _stock = new List<ItemSuper>();

    //--------------------------- 最大生成個数 ----------------------------------------
    [SerializeField, Range(10, 100)]
    private int _MaxSpawn;

    //--------------------------- 最低生成個数 ----------------------------------------
    [SerializeField, Range(0, 10)]
    private int _MinSpawn;


    #endregion  

    //#region Debug Properties 
    //-------------------------- デバッグ中か否か ---------------------------------------
    public bool _IsDebug = false;
    //#endregion  


    #region Unity Callbacks 

    // Start is called before the first frame update
    void Start()
    {
        //------------------------------------- 初期化 ---------------------------------------
        //アイテムの親オブジェクト　初期化
        _itemParent = new GameObject();
        _itemParent.name = "ItemParent" + this.gameObject.name;
        _itemParent.transform.parent = this.transform;
        _itemParent.transform.position = transform.TransformPoint(Vector3.zero);

        _saveSpawnInterval = _SpawnInterval;
        //-------------------------------------　初期化終わり ------------------------------

        //------------------------------------ 生成 ----------------------------------------

        for (int i = 0; i < _startItemNum; i++)
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
            _SpawnInterval = 100;   //スポーン間隔を遅く
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


        //デバッグ中ならスポーン間隔は短く
        _SpawnInterval = _IsDebug ? 1 : _saveSpawnInterval;

        _nearDis = 10000;
    }

    #endregion ----------------------------------------------------------------------------

    #region Accessor 

    public List<ItemSuper> GetStock()
    {
        return _stock;
    }

    /// <summary>
    /// GameObject名と一致するアイテムをリストからもシーン上からも消す
    /// </summary>
    /// <param name="objname"></param>
    public void RemoveItem(string objname)
    {
        for (int i = _stock.Count - 1; i >= 0; i--)
        {
            if (_stock[i]._object.name == objname)
            {
                GameObject tmp_obj = _stock[i]._object;
                _stock.RemoveAt(i);
                Destroy(tmp_obj);
            }
        }
    }

    /// <summary>
    /// 商品の返品処理
    /// </summary>
    /// <param name="item"></param>
    public void ReturnItem(ItemSuper item)
    {
        Vector3 pos = RandomPos();

        item._object = item.Init();
        item._object.transform.parent = _itemParent.transform;
        item._object.transform.position = pos;
        item._object.name += count++.ToString();
        _stock.Add(item);

        //ゲーム全体の在庫数に追加
        item.AddShopDistribution(item.GetNum());
    }

    #endregion  

    #region ItemInfo Methods  
    /// <summary>
    /// あるオブジェクトに一番近いアイテムの位置を返す
    /// </summary>
    /// <param name="objPos">対象のオブジェクト</param>
    /// <returns>一番近いアイテム</returns>
    public ItemSuper NearestItem(Vector3 objPos)
    {
        ItemSuper nearestItem = ItemSuper.Null;

        //在庫が1個しかなかったらそれを返す
        if (_stock.Count == 1) return _stock[0];

        foreach (var i in _stock)
        {
            if (i == ItemSuper.Null) continue;


            float tmp_dis = Vector3.Distance(i._object.transform.position, objPos);
            //より近いアイテムがあったら
            if (_nearDis > tmp_dis)
            {
                nearestItem = i;

                _nearDis = tmp_dis;
            }

        }
        return nearestItem;

    }

    #endregion 

    
    
    #region Generate Item Methods

    //------------------------- 命名用の変数 〇 + count.ToString() ----------------------------
    private int count = 1;

    /// <summary>
    /// Updateで実行　一定間隔でランダムにアイテムを生成
    /// </summary>
    private void SpawnItem()
    {
        _time += Time.deltaTime;

        //最大出現個数を上回ったら生成しない
        if (_stock.Count >= _MaxSpawn) _time = 0f;

        if (_time > _SpawnInterval)
        {
            //あらかじめ設定した値を追加するように
            GenerateItem();
            _time = 0;
        }
    }

    /// <summary>
    /// ランダムなアイテムを自動生成
    /// </summary>
    private void GenerateItem()
    {
        ItemSuper item = ItemSuper.RandomItem();

        Vector3 pos = RandomPos();

        item._object = item.Init();
        item._object.transform.parent = _itemParent.transform;
        item._object.transform.position = pos;
        item._object.name += count++.ToString();
        _stock.Add(item);

        //ゲーム全体の在庫数に追加
        item.AddShopDistribution(item.GetNum());

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



        return new Vector3(x, y, z) + transform.position;
    }

    #endregion  
}
