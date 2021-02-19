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

    Inventry _inventry;


    #endregion




    #region Unity Callbacks
    // Start is called before the first frame update
    void Start()
    {
        _ID = MonobitNetwork.player.ID;
    }

    // Update is called once per frame
    void Update()
    {

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

    public void Buy(Item.ItemName item , int num)
    {
        int sum = Item.ItemSuper.GetPriceSum(Item.ItemName.ToiletPaper, 10);
        if(_wallet.IsBuyable(sum))
            _wallet.Pay(sum);
        _inventry.AddItem(item, num);
    }

    #endregion

    #region Private Methods


    #endregion

}
