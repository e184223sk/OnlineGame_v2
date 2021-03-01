using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Item;
public class Inventry : MonoBehaviour
{
    #region Public Properties

    /// <summary>
    /// アイテムを格納する配列　読み取り専用なのでアクセスはメソッドからお願いします
    /// </summary>
    public readonly List<ItemSuper> _ItemList = new List<ItemSuper>();

    public int Length { get { return _slot; } }
        
    #endregion

    #region Private Properties

    //プレイヤーが所持するアイテム達　初期アイテムは全てnull


    //現在のインベントリのスロット数　デフォルトは 6 個
    private int _slot = 6;

    //現在のインベントリの空きスロット数　スロットの数だけfor文回して空きがあるかを確認するゲッター
    private int _remainSlot
    {
        get
        {
            int count = 0;
            for (int i = 0; i < _slot; i++)
            {
                count += (_ItemList[i] == ItemSuper.Null) ? 1 : 0;
            }
            return count;
        }   
        set { }
    }

    #endregion


    #region Public Methods

    public Inventry(List<ItemSuper> items)
    {
        _ItemList = new List<ItemSuper>(_slot);
        _ItemList.AddRange(Enumerable.Repeat(ItemSuper.Null, _slot));

        for (int i = 0; i < _ItemList.Count; i++)
        {
            //itemsが_itemListより要素が少ないとき
            if(i > items.Count - 1)  return;
            _ItemList[i] =  items[i];
        }
    }

    public Inventry() { }

    /// <summary>
    /// アイテム追加処理　すでに持っているアイテムは数を増やすだけ　超過分は次のスロットに格納
    /// </summary>
    public void AddItem(ItemSuper item, int num)
    {
        foreach (ItemSuper i in _ItemList)
        {
            if (i.GetName() == item.GetName())
            {
                int tmp_sum = i.GetNum() + item.GetNum(); ;         //とりあえず合計
                int over_num = tmp_sum - i._MaxNum;     //とりあえず超過数
                int remain_num = i._MaxNum - i.GetNum(); //とりあえず保有可能数

                //インベントリに追加したとき　最大所持数を超えるとき
                if (over_num > 0)
                {
                    //空きスロットがあるなら アイテムをインベントリに追加
                    if (_remainSlot > 0)
                    {
                        //超過した分だけ　次のスロットに格納
                        _ItemList[_slot - _remainSlot] = new ItemSuper(i.GetName(), i.GetPrice(), over_num);
                    }
                }
                //非超過分を足す
                i.AddNum(remain_num);
            }
        }
    }

    /// <summary>
    /// 渡されたアイテムをただ追加するだけ　超過分とかは考慮しない   店の在庫向け
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(ItemSuper item)
    {
        _ItemList.Add(item);
    }


    //アイテム削除処理
    public void DeleteItem(ItemName item, int num)
    {
        foreach (ItemSuper i in _ItemList)
        {
            //インベントリにアクセスしてアイテムを num 分減らす   プレイヤーのインベントリ向け
            if (i.GetName() == item.ToString())
            {
                //削除個数が超過した場合、所持数を 0 にしてインベントリから削除
                if (i.GetNum() > num)
                {
                    i.AddNum(i.GetNum());
                    _ItemList.Remove(i);
                }
                else
                {
                    i.AddNum(-num);
                }
            }
        }
    }


    /// <summary>
    /// インベントリの最大数を増やすメソッド　ItemSuper.Nullが最後尾に追加される
    /// </summary>
    /// <param name="num">増やす数</param>
    public void AddInventry(int num)
    {
        for (int i = 0; i < num; i++)
        {
            _ItemList.Add(ItemSuper.Null);
        }

        _slot += num;
    }


    /// <summary>
    /// インベントリの最大数を減らすメソッド　後ろから消していくが、すでにアイテムがある場合も問答無用で消す
    /// </summary>
    /// <param name="num">減らす数</param>
    public void DeleteInventry(int num)
    {
        for(int i = 0; i < num; i++)
        {
            _ItemList.RemoveAt(_ItemList.Count - 1);
        }
        _slot -= num;
    }

    

    #endregion

    #region Debug Methods

    public void DebugShowItems()
    {
        foreach(var i in _ItemList)
        {
            Debug.Log(i);
        }
    }


    #endregion
}
