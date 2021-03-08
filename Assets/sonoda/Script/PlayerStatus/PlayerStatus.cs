using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
public class PlayerStatus :MonobitEngine.MonoBehaviour
{
    #region Public Properties

    public int ShowMoney;


    #endregion

    #region Private Properties

    //MonobitNetwork上でのID
    private int _ID;


    //プレイヤーの体力　Hit Point
    private PlayerHP _HP;


    //private _weapon1; 銃制御スクリプト
    //private Weapon _weapon2;


    //プレイヤーの所持金
    private Wallet _wallet;

    //プレイヤーが持っているアイテム
    Inventry _inventry;

    //精算前に商品を入れるカゴ
    List<Item.ItemSuper> _basket = new List<Item.ItemSuper>();

    //現在入っている店の情報
    Shop _nowShop;

    //アイテムを取得可能な距離
    [SerializeField]
    float GettableDis;


    [SerializeField]
    List<Item.ItemSuper> ITEMS;
    #endregion




    #region Unity Callbacks
    // Start is called before the first frame update
    void Start()
    {
        _ID = MonobitNetwork.player.ID;

        _wallet = new Wallet(100000);

        _inventry = new Inventry();
    }

    // Update is called once per frame
    void Update()
    {
        ShowMoney = _wallet.GetMoney();

        Shopping();
        

    }

    #endregion

    #region MunRPC
    [MunRPC]
    public void Damage(int damage)
    {
        _HP.Damage(damage);
    }

    #endregion

    #region Public Methods
    public int GetID()
    {
        return _ID;
    }

   /* public void Buy(Item.ItemName item , int num)
    {
        int sum = Item.ItemSuper.GetPriceSum(Item.ItemName.ToiletPaper, 10);
        if(_wallet.IsBuyable(sum))
            _wallet.Pay(sum);
        _inventry.AddItem(item, num);
    }
   */
    public void EnterShop(GameObject shopObj)
    {
        
        _nowShop = shopObj.GetComponent<Shop>();
        //Debug.Log(_nowShop.name);
       // Debug.Log("配列の要素数" + _nowShop.GetStock().Count);

        ITEMS = _nowShop.GetStock();
        /*foreach(var i in _nowShop.GetStock())
        {
            Debug.Log(i._object.name +  i._object.transform.position);
        }*/
    }

    public void LeaveShop()
    {
        _nowShop = null;
        int sumPrice = 0;

        List<Item.ItemSuper> buyable_items = new List<Item.ItemSuper>();
        foreach(var i in _basket)
        {
            //代金以上のお金が財布に残っていたら
            if( _wallet.IsBuyable(sumPrice + Item.ItemSuper.GetPriceSum(i)))
            {
                sumPrice += Item.ItemSuper.GetPriceSum(i);
                buyable_items.Add(i);

                //プレイヤー全体の総所持数に追加
                i.AddPlayerDistribution(i.GetNum());
                //総在庫数からマイナス
                i.SubShopDistribution(i.GetNum());
            }
            else break;
        }
        _wallet.Pay(sumPrice);
        foreach(var i in buyable_items)
        {
            Item.ItemSuper tmp_item = _inventry.AddItem(i, 0);
            //アイテムがインベントリに収まらずあふれた時
            if (tmp_item != Item.ItemSuper.Null)
            {
                //超過分のお金の払い戻し
                _wallet.Receipt(Item.ItemSuper.GetPriceSum(tmp_item));


                //プレイヤー全体の総所持数からマイナス
                tmp_item.SubPlayerDistribution(tmp_item.GetNum());
                //総在庫数に追加
                _nowShop.ReturnItem(tmp_item);

                break;
            }
        }
    }
       

    #endregion

    #region Private Methods

    //Update内で実行 アクションを起こして最近接アイテムをカゴに入れる
    private void Shopping()
    {
        //入店時のみ処理
        if(_nowShop != null)
        {
            // A(仮) が押されたら　ボタンは後で変更
            if (Key.A.Down || Input.GetKeyDown(KeyCode.Space))
            {

                Debug.Log(_nowShop.gameObject.name);
                Item.ItemSuper item = _nowShop.NearestItem(transform.position);
                if (item == Item.ItemSuper.Null)
                {
                    Debug.Log("近くにないよ");
                    return;
                }
                

                //Debug.Log( item.GetName() +" : " + item._object.transform.position.ToString());

                //取得可能な距離にいたら
                if(GettableDis > Vector3.Distance(item._object.transform.position , gameObject.transform.position))
                {
                    Debug.Log("gettableItem is : " + item._object.name);
                    _basket.Add(item);
                    _nowShop.RemoveItem(item._object.name);
                }
            }
        }
    }

    #endregion

}
