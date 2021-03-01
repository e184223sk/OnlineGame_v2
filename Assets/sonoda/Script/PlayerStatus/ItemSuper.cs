﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{

    public class ItemSuper : MonoBehaviour
    {
        #region Public Properties

        //所持できる最大数
        public readonly int _MaxNum = 100;

        //初期値　空白のアイテム
        public static readonly ItemSuper Null = new ItemSuper();


        //アイテムのゲームオブジェクト
        public GameObject _object = null;



        #endregion

        #region Protected Properties -----------------------------------------------

        //アイテムの値段
        protected int _price;

        //アイテムの個数
        protected int _num;

        //アイテム名
        protected string _name;

        //プレハブ名 アイテムを生成するときにResourceから呼び出す時に使う
        protected string _prefabName;

        //生成したオブジェクトに付けるスクリプト
        protected ItemSuper _addComponent;
        #endregion ----------------------------------------------------------------

        #region Private Properties




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
        public ItemSuper(string name = "", int price = 0, int num = 0)
        {
            _name = name;
            _price = price;
            _num = num;
        }


        #region Getter---------------------------------------------------------
        public int GetPrice()
        {
            return _price;
        }
        public int GetNum()
        {
            return _num;
        }

        public string GetName()
        {
            return _name;
        }

        /// <summary>
        /// このアイテムを入手する処理
        /// </summary>
        /// <returns></returns>

        public ItemSuper GetItem()
        {
            return this;
        }
        #endregion ----------------------------------------------------------

        #region Setter -------------------------------------------------
        public void Set(string name = "", int price = 0, int num = 0)
        {
            _name = name;
            _price = price;
            _num = num;
        }
        public void SetPrice(int price)
        {
            _price = price;
        }
        public void SetName(string name)
        {
            _name = name;
        }
        #endregion --------------------------------------------------------

        public void AddNum(int num)
        {
            _num += num;
        }


        //アイテムを購入する処理
        public void Buy(int num)
        {
            _num -= num;
        }


        /// <summary>
        /// このアイテムを生成する処理
        /// </summary>
        /// <returns></returns>
        public GameObject Init()
        {
            return Instantiate(Resources.Load(_prefabName), Vector3.zero, Quaternion.identity) as GameObject;
        }

        public override string ToString()
        {
            return "商品名：" + _name + "    個数：" + _num.ToString() + "　　　値段：" + _price;
        }
        #endregion -------------------------------------------------------------------------------

        #region Static Properties -------------------------------------------------------------------

        /// <summary>
        /// 引数で指定したアイテムを指定した分だけ買ったときの合計金額を返す
        /// </summary>
        /// <param name="item">列挙型のアイテム名</param>
        /// <param name="num">指定する個数</param>
        /// <returns></returns>
        /*public static int GetPriceSum(ItemName item, int num)
        {
            switch (item)
            {

                case ItemName.ToiletPaper:
                    return Item.ToiletPaper.ToiletPaperDefault.GetPrice() * num;

            }
            return 0;

        }*/

        public static int GetPriceSum(ItemSuper item)
        {
            return item.GetPrice() * item.GetNum();
        }


        //ランダムなアイテムの名前を生成
        public static Item.ItemName RandomItemName()
        {
            var names = System.Enum.GetNames(typeof(ItemName));
            
            return (ItemName)System.Enum.Parse(typeof(ItemName) ,names[Random.Range(0, names.Length)]);

        }

        public static ItemSuper RandomItem()
        {
            ItemName name = RandomItemName();
            switch (name.ToString())
            {
                case "Mask":
                    Mask tmp_mask = new Mask("不織布マスク", 200, 30);
                    tmp_mask._object = Resources.Load(tmp_mask._prefabName) as GameObject;

                    return tmp_mask;



                case "ToiletPaper":
                    ToiletPaper tmp_ToiletPaper = new ToiletPaper("トイレットペーパー", 100, 50);
                    tmp_ToiletPaper._object = Resources.Load(tmp_ToiletPaper._prefabName) as GameObject;

                    return tmp_ToiletPaper;
            }


            return ItemSuper.Null;
        }


        public static bool operator == (ItemSuper a , ItemSuper b)
        {
            return (a.GetPrice() == b.GetPrice()) && (a.GetNum() == b.GetNum()) && (a.GetName() == b.GetName());
        }
        public static bool operator !=(ItemSuper a, ItemSuper b)
        {
            return (a.GetPrice() != b.GetPrice()) || (a.GetNum() != b.GetNum()) || (a.GetName() != b.GetName());
        }

        #endregion ---------------------------------------------------------------------------------

    }

    public enum ItemName
    {
        ToiletPaper,
        Mask
    }
}


