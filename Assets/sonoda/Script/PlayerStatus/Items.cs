using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Item
{
    public class ToieltPaper : ItemSuper
    {

        #region Static Properties
        //デフォルトの設定
        public static ToieltPaper ToiletPaperDefault = new ToieltPaper("トイレットペーパー", 100, 50);

        #endregion

        #region Public Methods

        public ToieltPaper(string name = "", int price = 0, int num = 0) : base(name, price, num)
        {
            _name = name;
            _price = price;
            _num = num;
        }


        #endregion
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }
    }





    public enum ItemName
    {
        ToiletPaper
    }
}
