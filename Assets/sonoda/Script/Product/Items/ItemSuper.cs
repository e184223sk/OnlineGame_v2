using System.Collections;
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
        public static  ItemSuper Null = new ItemSuper();


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

        
        //アイテムUIとして表示する2Dイメージ
        [SerializeField]
        protected Sprite _ItemImage;

        #endregion ----------------------------------------------------------------

        #region Private Properties




        #endregion

        #region Static Properties ----------------------------------------------------------------

        //プレイヤーが所持しているアイテムの総量
        protected static int _PlayerDistribution;


        //店が保有する在庫の総数
        protected static int _ShopDistribution;

        //流通量に応じて変動する価格の計算式は
        //買値が _PlayerDistribution / _ShopDistribution   * price + price;    プレイヤーの所持数　/　在庫の総数　×　値段　+　値段
        //売値が _ShopDistribution   / _PlayerDistribution * price + price;    在庫の総数　/　プレイヤーの所持数　×　値段　+　値段


        #endregion ----------------------------------------------------------------

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

        /// <summary>
        /// プレイヤー間に出回るほど高く　店の在庫が増えるほど安くなる
        /// </summary>
        /// <returns>プレイヤーの買値</returns>
        public int GetBuyPrice()
        {
            return _PlayerDistribution / _ShopDistribution * _price + _price;
        }

        /// <summary>
        /// 買値＊
        /// </summary>
        /// <returns>プレイヤーの売値(色付き)</returns>
        public int GetSellPrice()
        {
            return (int)((_ShopDistribution / _PlayerDistribution * _price + _price) * 1.2f);
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

        public Sprite GetSprite()
        {
            return _ItemImage;
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


        /// <summary>
        /// 各店の在庫の総数に追加
        /// </summary>
        /// <param name="num"></param>
        public  void AddShopDistribution(int num)
        {
            _ShopDistribution += num;
        }

        /// <summary>
        /// 各店の在庫の総数から除く
        /// </summary>
        /// <param name="num"></param>
        public  void SubShopDistribution(int num)
        {
            _ShopDistribution += num;
        }

        /// <summary>
        /// プレイヤーの総所持数に追加
        /// </summary>
        /// <param name="num"></param>
        public  void AddPlayerDistribution(int num)
        {
            _PlayerDistribution += num;
        }

        /// <summary>
        /// プレイヤーの総所持数から除く
        /// </summary>
        /// <param name="num"></param>
        public  void SubPlayerDistribution(int num)
        {
            _PlayerDistribution += num;
        }

        #endregion -------------------------------------------------------------------------------

        #region Static Methods -------------------------------------------------------------------

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

        ///<summary>
        ///アイテムの購入金額    買値 ＊　個数
        ///</summary>
        public static int GetPriceSum(ItemSuper item)
        {
            return item.GetBuyPrice() * item.GetNum();
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


