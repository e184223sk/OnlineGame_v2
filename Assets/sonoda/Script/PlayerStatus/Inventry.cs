﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Item;
public class Inventry : MonoBehaviour
{
    #region Inventry Properties  

    /// <summary>
    /// --------------------------------- アイテムを格納する配列 ----------------------------------------------
    /// </summary>
    public List<ItemSuper> _ItemList = new List<ItemSuper>();

    // -------------------------- 現在のインベントリのスロット数　デフォルトは 6 個 ---------------------------
    private int _slot = 6;

    // --------------------------------- インベントリの最大スロット数 -----------------------------------------
    private int _MaxSlot = 24;

    // ------------------------------- インベントリの空きスロット数 ---------------------------------------------
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

    #region InventryUI Properties 



    public InventryUI _inventryUI;

    #endregion  

    #region Constructer Initializer

    /// <summary>
    /// 生成と同時に代入するとき    使わないかも  デフォルトのサイズ6を超えたら入れない
    /// </summary>
    /// <param name="items"></param>
    public Inventry(List<ItemSuper> items)
    {
        InitList(_slot);
        for (int i = 0; i < _ItemList.Count; i++)
        {
            //itemsが_itemListより要素が少ないとき
            if (i > items.Count - 1) return;
            _ItemList[i] = items[i];
        }
        _inventryUI = new InventryUI("InventryUI");
    }

    //何も指定せず生成
    public Inventry()
    {
        _ItemList = new List<ItemSuper>();
        _inventryUI = new InventryUI("InventryUI");
    }

    //インベントリのスロット数を指定して生成
    public Inventry(int slot)
    {
        InitList(slot);
        _inventryUI = new InventryUI("InventryUI");
    }

    private void InitList(int slot)
    {
        _ItemList = new List<ItemSuper>(slot);
        _ItemList.AddRange(Enumerable.Repeat(ItemSuper.Null, slot));
    }

    #endregion 　

    #region Inventry Methods

    /// <summary>
    /// //超過した分だけ　次のスロットに格納アイテム追加処理　すでに持っているアイテムは数を増やすだけ　超過分は次のスロットに格納　それでもあふれたアイテムを返す
    /// </summary>
    /// <param name="item">追加するアイテム</param>
    /// <param name="num">使わぬ</param>
    /// <returns>超過して入らなかったアイテム　超過しなかった場合はItemSuper.Nullが入る</returns>
    public ItemSuper AddItem(ItemSuper item, int num)
    {
        if (_ItemList.Count == 0)
        {
            _ItemList.Add(item);
            return ItemSuper.Null;
        }
        ItemSuper overflowItem = ItemSuper.Null;
        foreach (ItemSuper i in _ItemList)
        {
            //同じアイテム名のアイテムが存在し、かつ上限まで入っていない時
            if (i.GetName() == item.GetName() && i._MaxNum != i.GetNum())
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

                        _ItemList[_slot - _remainSlot] = new ItemSuper(i.GetName(), i.GetPrice(), over_num);
                        //非超過分を足す
                        i.AddNum(remain_num);
                    }
                    else
                    {
                        //超過分を格納
                        item = new ItemSuper(i.GetName(), i.GetPrice(), over_num);
                    }
                }
            }
        }

        _ItemList.Add(item);

        return overflowItem;
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
    /// アイテムを全て削除する
    /// </summary>
    public void ClearItems()
    {
        int tmp_slot = _ItemList.Count;
        InitList(tmp_slot);
    }

    /// <summary>
    /// インベントリの最大数を増やすメソッド　ItemSuper.Nullが最後尾に追加される
    /// </summary>
    /// <param name="num">増やす数</param>
    public void AddInventrySlot(int num)
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
    public void DeleteInventrySlot(int num)
    {
        for (int i = 0; i < num; i++)
        {
            _ItemList.RemoveAt(_ItemList.Count - 1);
        }
        _slot -= num;
    }

    #endregion 

    public void ShowInventryUI()
    {
        _inventryUI.ShowInventryUI(_ItemList);

    }

    public void HideInventryUI()
    {
        _inventryUI.HideInventryUI();
    }

    #region Debug Methods

    public void DebugShowItems()
    {
        foreach (var i in _ItemList)
        {
            Debug.Log(i);
        }
    }


    #endregion  
}
