using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Item;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{

    #region 
    
    //商品名
    private Text _name;

    //商品の個数
    private Text _num;

    private Image _image;   

    #endregion

    #region Public Methods

    public void SetUI(ItemSuper item)
    {
        if(item == ItemSuper.Null)
        {
            _name.text = "";
            _num.text = "";
        }
        else
        {
            _name.text = item.GetName();
            _num.text = "×" + item.GetNum().ToString();
            Debug.Log(item.GetSprite().name);
            _image.sprite = item.GetSprite();

        }
    }


    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _name = transform.GetChild(0).GetComponent<Text>();
        _num = transform.GetChild(1).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
