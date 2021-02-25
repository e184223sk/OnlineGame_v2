﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public class ToiletPaper : ItemSuper
    {
        #region Static Properties
        //デフォルトの設定
        public static readonly ToiletPaper ToiletPaperDefault = new ToiletPaper("トイレットペーパー", 100, 50);


        #endregion

        #region Public Methods

        public ToiletPaper(string name = "", int price = 0, int num = 0) : base(name, price, num)
        {
            _name = name;
            _price = price;
            _num = num;
            _prefabName = "TOILETPAPER";
        }


        #endregion
    }
}
