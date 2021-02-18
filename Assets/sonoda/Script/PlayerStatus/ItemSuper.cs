using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSuper : MonoBehaviour
{
    #region Public Properties

    //所持できる最大数
    public readonly int _MaxNum;

    //初期値　空白のアイテム
    public static readonly ItemSuper Null = new ItemSuper();
    
    #endregion

    #region Private Properties

    //アイテムの値段
    private int _price;

    //アイテムの個数
    private int _num;

    //アイテム名
    private string _name;

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

    #region Public Methods

    public ItemSuper(string name  = "", int price = 0, int num = 0)
    {
        _name = name;
        _price = price;
        _num = num;
    }



    public int GetPrice()
    {
        return _price;
    }

    public void SetPrice(int price)
    {
        _price = price;
    }

    public int GetNum()
    {
        return _num;
    }

    public void AddNum(int num)
    {
        _num += num;
    }

    //名前のゲッター
    public string GetName()
    {
        return _name;
    }
    public void SetName(string name)
    {
        _name = name;
    }

    //アイテムを購入する処理
    public void Buy(int num)
    {
        _num -= num;
    }


    public override string ToString()
    {
        return "商品名：" + _name + "    個数：" + _num.ToString() + "　　　値段：" + _price;
    }
    #endregion


}
