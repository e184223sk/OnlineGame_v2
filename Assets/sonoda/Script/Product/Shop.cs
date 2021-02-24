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
    [Range(0,180)]
    public float _SpawnInterval ;
    
    
    #endregion

    #region Private Properties

    //この店の在庫
    private Inventry _stock;

    //商品を配置する範囲
    [SerializeField]
    private Vector3 _productArea;

    //店の座標
    private Vector3 _position;

    //店のトランスフォーム
    private Transform _transform;

    //アイテムの親オブジェクト　（空）
    private GameObject _itemParent;

    //経過時間
    private float _time;

    //最低生成個数
    [SerializeField ,Range(0,50)]
    private int _MinSpawn;

    //最大生成個数
    [SerializeField, Range(50, 100)]
    private int _MaxSpawn;

    #endregion


    #region Unity Callbacks

    // Start is called before the first frame update
    void Start()
    {
        //------------------------------------- 初期化 ---------------------------------------
        //最初にスポーンさせるアイテムのリスト
        var list = new List<Item.ItemSuper>()
        {
            Item.Mask.MaskDefault,
            Item.ToieltPaper.ToiletPaperDefault
        };

        //在庫の初期化
        _stock = new Inventry( list );

        //アイテムの親オブジェクト　初期化
        _itemParent = new GameObject();
        _itemParent.name = "ItemParent";


        //position　代入
        _position = this.transform.position;
        //-------------------------------------　初期化終わり ------------------------------

        int[] a = new int[1];
        //------------------------------------ 生成 ----------------------------------------

        for (int i = 0; i < _stock._ItemList.Count; i++)
        {
            if (_stock._ItemList[i] != Item.ItemSuper.Null)
            {
               GameObject tmp_obj = _stock._ItemList[i].Init(RandomPos());
                tmp_obj.transform.parent = _itemParent.transform;
            }
        }

        //------------------------------------ 生成終わり ----------------------------------------

        Debug.Log(Item.ItemSuper.RandomItemName());

    }

    // Update is called once per frame
    void Update()
    {
        SpawnItem();
    }

    #endregion



    #region Private Methods

    /// <summary>
    /// Updateで実行　一定間隔でランダムにアイテムを生成
    /// </summary>
    private void SpawnItem()
    {
        _time += Time.deltaTime;

        if(_time  > _SpawnInterval)
        {
            _stock.AddItem(ItemSuper.RandomItemName());
            GameObject tmp_obj= _stock._ItemList[_stock._ItemList.Count - 1].Init(RandomPos());
            tmp_obj.transform.parent = _itemParent.transform;
            
            _time = 0;
        }

    }


    /// <summary>
    /// 商品を生成する位置をランダムで生成
    /// </summary>
    /// <returns></returns>
    private Vector3 RandomPos()
    {
        float x, y, z;
        x = Random.Range(-_productArea.x / 2, _productArea.x / 2) + _position.x;
        y = 1;
        z = Random.Range(-_productArea.z / 2, _productArea.z / 2) + _position.z;

        return transform.InverseTransformPoint(new Vector3(x, y, z));
    }

    #endregion
}
