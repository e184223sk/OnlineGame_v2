using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{

    public class ItemSuper : MonoBehaviour
    {
        //#region ItemSuper Definition

        //------------------------------------ 初期値　空白のアイテム ---------------------------------------------
        public static ItemSuper Null = new ItemSuper();

        //#endregion

        #region ItemStatus Properties 

        //------------------------------------- 所持できる最大数 ------------------------------------------------
        public readonly int _MaxNum = 100;


        //------------------------------------- アイテムのゲームオブジェクト -------------------------------------
        public GameObject _object = null;


        //------------------------------------- アイテムの値段 ----------------------------------------------------
        protected int _price;


        //------------------------------------- アイテムの個数 -----------------------------------------------------
        protected int _num;


        //------------------------------------ アイテム名 ---------------------------------------------------------
        protected string _name;


        //----------------------- プレハブ名 アイテムを生成するときにResourceから呼び出す時に使う ----------------
        protected string _prefabName;

        //-------------------------------------- アイテムUIとして表示する2Dイメージ --------------------------------
        [SerializeField]
        protected Sprite _ItemImage;

        #endregion  

        #region Price Management Properties 

        //--------------------------------------- プレイヤーが所持しているアイテムの総量 --------------------------
        protected static int _PlayerDistribution;


        //--------------------------------------- 店が保有する在庫の総数 -------------------------------------------
        protected static int _ShopDistribution;


        //--------------------------------------- 急激な値段の増減を丸めこむ値 -------------------------------------
        private float _priceRound = 1.2f;

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

        
        public ItemSuper(string name = "", int price = 0, int num = 0)
        {
            _name = name;
            _price = price;
            _num = num;
        }


        #region Getter 
        public int GetPrice() { return _price; }

        /// <summary>
        /// プレイヤーが買うときの値段 プレイヤー間に出回るほど高く　店の在庫が増えるほど安くなる
        /// </summary>
        /// <returns>プレイヤーの買値</returns>
        public int GetBuyPrice()
        {
            float f = _PlayerDistribution / _ShopDistribution;
            float b = (f > 0 ? f / _priceRound : f * _priceRound) * _price;

            return (int)b + _price;
        }

        /// <summary>
        /// プレイヤーが売るときの値段　プレイヤー間に出回っていないほど高く　店の在庫が減るほど安くなる(他の人が持ってて需要が減るから)
        /// </summary>
        /// <returns>プレイヤーの売値(色付き)</returns>
        public int GetSellPrice(float profitRate)
        {
            float f = _ShopDistribution / _PlayerDistribution;
            float c = ((f > 0 ? f / _priceRound : f * _priceRound) * _price * profitRate);

            return (int)c + _price;
        }

        public int GetNum() { return _num; }

        public string GetName() { return _name; }

        /// <summary>
        /// このアイテムを入手する処理
        /// </summary>
        /// <returns></returns>
        public ItemSuper GetItem() { return this; }

        public Sprite GetSprite() { return _ItemImage; }

        //アイテムの購入金額    買値 ＊　個数
        public static int GetPriceSum(ItemSuper item) { return item.GetBuyPrice() * item.GetNum(); }

        #endregion 

        #region Setter  
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
        #endregion  

        #region Price Caluculator
        public void AddNum(int num) { _num += num; }

    


        /// <summary>
        /// 各店の在庫の総数に追加
        /// </summary>
        /// <param name="num"></param>
        public void AddShopDistribution(int num) { _ShopDistribution += num; }

        /// <summary>
        /// 各店の在庫の総数から除く
        /// </summary>
        /// <param name="num"></param>
        public void SubShopDistribution(int num) { _ShopDistribution += num; }

        /// <summary>
        /// プレイヤーの総所持数に追加
        /// </summary>
        /// <param name="num"></param>
        public void AddPlayerDistribution(int num) { _PlayerDistribution += num; }

        /// <summary>
        /// プレイヤーの総所持数から除く
        /// </summary>
        /// <param name="num"></param>
        public void SubPlayerDistribution(int num) { _PlayerDistribution += num; }


        #endregion

        #region Item Initialize Methods

        /// <summary>
        /// このアイテムを生成する処理
        /// </summary>
        /// <returns></returns>
        public GameObject Init() { return Instantiate(Resources.Load(_prefabName), Vector3.zero, Quaternion.identity) as GameObject; }

        //ランダムなアイテムの名前を生成
        public static Item.ItemName RandomItemName()
        {
            var names = System.Enum.GetNames(typeof(ItemName));

            return (ItemName)System.Enum.Parse(typeof(ItemName), names[Random.Range(0, names.Length)]);

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



        #endregion
        #region Overload Methods

        public override string ToString() { return "商品名：" + _name + "    個数：" + _num.ToString() + "　　　値段：" + _price; }
        public static bool operator ==(ItemSuper a, ItemSuper b)
        {
            return (a.GetPrice() == b.GetPrice()) && (a.GetNum() == b.GetNum()) && (a.GetName() == b.GetName());
        }
        public static bool operator !=(ItemSuper a, ItemSuper b)
        {
            return (a.GetPrice() != b.GetPrice()) || (a.GetNum() != b.GetNum()) || (a.GetName() != b.GetName());
        }
        #endregion
    }

    public enum ItemName
    {
        ToiletPaper,
        Mask
    }
}


