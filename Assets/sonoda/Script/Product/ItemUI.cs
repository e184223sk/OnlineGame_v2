using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Item;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    //商品の個数
    private Text _num;

    private Image _image;

    private Sprite _rockImage;

    public void SetUI(ItemSuper item)
    {
        if(item == ItemSuper.Null)
        {
            _num.text = "";
            _image.sprite = _rockImage;
        }
        else
        {
            _num.text = item.GetNum().ToString();
            _image.sprite = item.GetSprite();
        }
    }


    public void Initialize()
    {
        _rockImage = Resources.Load("Rocked") as Sprite;
        _image = GetComponent<Image>();
        _num = transform.GetChild(0).GetComponent<Text>();
    }
}
