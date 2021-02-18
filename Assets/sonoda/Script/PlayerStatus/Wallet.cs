using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの所持金のクラス
/// </summary>
public class Wallet : MonoBehaviour
{
    #region Public Properties

    

    #endregion

    #region Private Properties

    //所持金　
    private int _money;




    #endregion

    #region Public Methods

    //コンストラクタ 　所持金のデフォルトは 1 万円
    public Wallet(int money = 10000)
    {
        _money = money;
    }


    //商品が買えるだけのお金があるか
    public bool IsBuyable(int productPrice)
    {
        return productPrice < _money;
    }

    //財布から支払う
    public void Pay(int productPrice)
    {
        _money -= productPrice;
    }


   //売上金を財布に入れる
    public void Receipt(int profit)
    {
        _money += profit;
    }

    //所持金のゲッター
    public int GetMoney()
    {
        return _money;
    }
    #endregion

}
