using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
public class PlayerStatus :MonobitEngine.MonoBehaviour
{
    #region Public Properties

    //MonobitNetwork上でのID
    private int _ID;

    #endregion

    #region Private Properties

    //プレイヤーの体力　Hit Point
    private PlayerHP _HP;


    //private _weapon1; 銃制御スクリプト
    //private Weapon _weapon2;


    //プレイヤーの所持金
    private Wallet _wallet;

    //プレイヤーが持っているアイテム
    Inventry _inventry;

    //精算前に商品を入れるカゴ
    Inventry _basket;

    //現在入っている店の情報
    Shop _nowShop;

    //アイテムを取得可能な距離
    [SerializeField]
    float GettableDis;

    #endregion




    #region Unity Callbacks
    // Start is called before the first frame update
    void Start()
    {
        _ID = MonobitNetwork.player.ID;

        _basket = new Inventry();
    }

    // Update is called once per frame
    void Update()
    {
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
    public void EnterShop(Shop shop)
    {
        _nowShop = shop;
    }

    public void LeaveShop()
    {
        _nowShop = null;
        int sumPrice = 0;

        List<Item.ItemSuper> tmp_items = new List<Item.ItemSuper>();
        foreach(var i in _basket._ItemList)
        {
            sumPrice += Item.ItemSuper.GetPriceSum(i);
            tmp_items.Add(i);
            if (!_wallet.IsBuyable(sumPrice)) break;
        }
        _wallet.Pay(sumPrice);
        foreach(var i in tmp_items)
        {
            _inventry.AddItem(i);
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
                Item.ItemSuper item = _nowShop.NearestItem(gameObject.transform.position);


                //取得可能な距離にいたら
                if(GettableDis > Vector3.Distance(item._object.transform.position , gameObject.transform.position))
                {
                    _basket.AddItem(item);
                    Destroy(item._object);
                }
            }
        }
    }

    #endregion

}
