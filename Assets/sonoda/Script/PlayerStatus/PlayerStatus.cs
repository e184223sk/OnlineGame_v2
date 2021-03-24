using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
public class PlayerStatus : MonobitEngine.MonoBehaviour
{
    #region Status Properties
    //----------------------------- MonobitNetwork上でのID ----------------------------
    private int _ID;

    //----------------------------- プレイヤーの体力　Hit Point -------------------------
    private PlayerHP _HP;

    //-----------------------------プレイヤーの所持金 ----------------------------------
    private Wallet _wallet;
    
    //-----------------------------プレイヤーが持っているアイテム -----------------------
    Inventry _inventry;

    //----------------------------- インベントリを開いているか--------------------------
    private bool _IsOpen = false;

    //private _weapon1; 銃制御スクリプト
    //private Weapon _weapon2;

    #endregion

    #region Shopping Properties

    //----------------------------- 精算前に商品を入れるカゴ -----------------------------
    List<Item.ItemSuper> _basket = new List<Item.ItemSuper>();

    //----------------------------- 現在入っている店の情報 -------------------------------
    Shop _nowShop;

    //-----------------------------アイテムを取得可能な距離 -----------------------------
    [SerializeField]
    float GettableDis;

    //----------------------------- 売るときの倍率　-------------------------------------
    [SerializeField, Lang_Jp("利益率")]
    private float _profitRate = 1.2f;

    private InventryUI _basketUI;

    #endregion

    #region Debug Properties

    //--------------------------------- 所持金を表示するためだけの変数 --------------------------------
    public int ShowMoney;

    #endregion

    #region Unity Callbacks
    // Start is called before the first frame update
    void Start()
    {

        _ID = MonobitNetwork.player.ID;

        _wallet = new Wallet(100000);

        _inventry = new Inventry();

        _basketUI = new InventryUI("BasketUI");
    }

    // Update is called once per frame
    void Update()
    {
        ShowMoney = _wallet.GetMoney();

        Shopping();

        if (Input.GetKeyDown(KeyCode.E))
        {
            _IsOpen = !_IsOpen;
            if (_IsOpen) _inventry.ShowInventryUI();
            else _inventry.HideInventryUI();
        }



        if (_nowShop)   _basketUI.ShowInventryUI(_basket);
        else            _basketUI.HideInventryUI();

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

    public void EnterShop(GameObject shopObj)
    {

        _nowShop = shopObj.GetComponent<Shop>();
        //Debug.Log(_nowShop.name);
        // Debug.Log("配列の要素数" + _nowShop.GetStock().Count);

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
        foreach (var i in _basket)
        {
            //代金以上のお金が財布に残っていたら
            if (_wallet.IsBuyable(sumPrice + Item.ItemSuper.GetPriceSum(i)))
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
        foreach (var i in buyable_items)
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
        _basket.Clear();
    }

    #endregion

    #region Shopping Methods

    //Update内で実行 アクションを起こして最近接アイテムをカゴに入れる
    private void Shopping()
    {
        //入店時のみ処理
        if (_nowShop != null)
        {
            // A(仮) が押されたら　ボタンは後で変更     キーボードは「 Z 」
            if (Key.A.Down)
            {
                //プレイヤーがアイテムを買う処理 ---------------------------------------------------------------
                if (_nowShop._IsSalesShop)
                {
                    Debug.Log(_nowShop.gameObject.name);
                    Item.ItemSuper item = _nowShop.NearestItem(transform.position);

                    //アイテムを取得したときお金が足りなくなるなら買えない
                    if (!_wallet.IsBuyable(GetBasketSum() + Item.ItemSuper.GetPriceSum(item)))
                    {
                        Debug.Log("お金足りないぞ");
                        return;
                    }
                    if (item == Item.ItemSuper.Null)
                    {
                        Debug.Log("近くにないよ");
                        return;
                    }

                    //Debug.Log( item.GetName() +" : " + item._object.transform.position.ToString());

                    //取得可能な距離にいたら
                    if (GettableDis > Vector3.Distance(item._object.transform.position, gameObject.transform.position))
                    {
                        Debug.Log("gettableItem is : " + item._object.name);
                        _basket.Add(item);
                        _nowShop.RemoveItem(item._object.name);
                    }
                }
                else  //プレイヤーがアイテムを売る処理 ------------------------------------------------------------
                {

                }

            }
        }

    }

    /// <summary>
    /// カゴの中のアイテムの総購入金額
    /// </summary>
    /// <returns></returns>
    private int GetBasketSum()
    {
        //カゴの中身が空なら 0円
        if (_basket.Count == 0) return 0;

        //カゴの中のアイテムの総購入金額
        int sum = 0;
        foreach (var i in _basket) sum += Item.ItemSuper.GetPriceSum(i);

        return sum;
    }


    /// <summary>
    /// プレイヤーが所持しているアイテムの総売却金額
    /// </summary>
    /// <returns></returns>
    private int GetItemListSum()
    {
        int sum = 0;
        //
        foreach(var i in _inventry._ItemList)   sum += i.GetSellPrice(_profitRate);
        return sum;
    }
    #endregion --------------------------------------------------------------------------------

}
