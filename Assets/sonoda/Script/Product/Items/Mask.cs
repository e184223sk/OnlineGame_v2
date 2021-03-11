using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public class Mask : ItemSuper
    {
        public static Mask MaskDefault = new Mask("不織布マスク", 200, 30);


        public Mask(string name, int price, int num)
        {
            _name = name;
            _price = price;
            _num = num;
            _prefabName = "MASK";
            _ItemImage = Resources.Load<Sprite>("MASKIMAGE");
        }
    }
}