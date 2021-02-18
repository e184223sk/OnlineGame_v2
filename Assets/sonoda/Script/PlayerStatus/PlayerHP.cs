using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;


public class PlayerHP :MonobitEngine.MonoBehaviour
{
    #region Public Properties




    #endregion



    #region Private Properties

    //プレイヤーの体力
    private int _hitPoint;




    #endregion


    #region Public Methods

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="hp">初期HP</param>
    public PlayerHP(int hp)
    {
        _hitPoint = hp;
    }
    /// <summary>
    /// 外部からの攻撃
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void Damage(int damage)
    {
        _hitPoint -= damage;
    }

    /// <summary>
    /// 回復メソッド
    /// </summary>
    /// <param name="heal">回復量</param>
    public void Heal(int heal)
    {
        _hitPoint += heal;
    }
    
    public int GetHP()
    {
        return _hitPoint;
    }

    #endregion


}
