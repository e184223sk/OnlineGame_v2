using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Item
{
    public class ToieltPaper : ItemSuper
    {

        #region Static Properties
        //デフォルトの設定
        public static readonly ToieltPaper ToiletPaperDefault = new ToieltPaper("トイレットペーパー", 100, 50);


        #endregion

        #region Public Methods

        public ToieltPaper(string name = "", int price = 0, int num = 0) : base(name, price, num)
        {
            _name = name;
            _price = price;
            _num = num;
            _prefabName = "TOILETPAPER";
        }


        #endregion
    }
    public class Mask : ItemSuper
    {
        public static Mask MaskDefault = new Mask("不織布マスク", 200, 30);


        public Mask( string name , int price , int num)
        {
            _name = name;
            _price = price;
            _num = num;
            _prefabName = "MASK";
        }
    }





    public enum ItemName
    {
        ToiletPaper,
        Mask
    }
}
