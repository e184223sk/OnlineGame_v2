using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Item;
public class Clerk : MonoBehaviour
{

    #region Public Properties



    #endregion


    #region Private Properties 

    //ショップの商品だな　仮想
    private Inventry _inventry = new Inventry(
        new List<ItemSuper>() {
        Item.ToieltPaper.ToiletPaperDefault,
        Item.ToieltPaper.ToiletPaperDefault,
        Item.ToieltPaper.ToiletPaperDefault,
        Item.ToieltPaper.ToiletPaperDefault,
        Item.ToieltPaper.ToiletPaperDefault,
        Item.ToieltPaper.ToiletPaperDefault}
        );

    //プレイヤーがいる範囲を検知する中心点
    [SerializeField]
    private Vector3 _targetPos;

    //プレイヤーがいる範囲を検知する　半径
    [SerializeField]
    private float _targetRadius;









    #endregion


    #region Unity Callbacks
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion



    #region Private Methods

    //プレイヤーが一定範囲にいるか調べる関数
   /* private bool IsPlayer()
    {
        if()
    }*/


    #endregion
}
