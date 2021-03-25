using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventryUI : MonoBehaviour
{

    [SerializeField]
    // --------------------------------- インベントリのUI　親オブジェクト --------------------------------------
    private GameObject _inventryUI;

    // --------------------------------- インベントリのUI　アイテムUIの親 -------------------------------------
    private GameObject _products;

    // ------------------------------------- アイテムUIの配列 ------------------------------------------------
    private ItemUI[] _itemUIs;


    public InventryUI(string name)
    {
        GameObject tmp_ui = Resources.Load(name) as GameObject;
        _inventryUI = Instantiate(tmp_ui);
        _products = _inventryUI.transform.GetChild(0).GetChild(0).GetChild(1).gameObject;

        GameObject[] tmp_objs = GetAllChildObj(_products.transform);
        _itemUIs = new ItemUI[tmp_objs.Length];
        for (int i = 0; i < _itemUIs.Length; i++)
        {
            _itemUIs[i] = tmp_objs[i].GetComponent<ItemUI>();
            _itemUIs[i].Initialize();
        }
        _inventryUI.SetActive(false);
    }


    private GameObject[] GetAllChildObj(Transform parent)
    {
        GameObject[] Objs = new GameObject[parent.childCount];
        for (int i = 0; i < parent.childCount; i++)
        {
            Objs[i] = parent.GetChild(i).gameObject;
        }
        return Objs;
    }

    private void UISet()
    {
        _inventryUI = GameObject.Find("InventryUI");
        _products = GameObject.Find("Products");
        GameObject[] tmp_objs = GetAllChildObj(_products.transform);
        _itemUIs = new ItemUI[tmp_objs.Length];
        for (int i = 0; i < _itemUIs.Length; i++)
        {
            _itemUIs[i] = tmp_objs[i].GetComponent<ItemUI>();
        }
        _inventryUI.SetActive(false);
    }

    public void HideInventryUI()
    {
        _inventryUI.SetActive(false);
    }

    /// <summary>
    /// インベントリを開く
    /// </summary>
    public void ShowInventryUI(List< Item.ItemSuper> ItemList)
    {
        _inventryUI.SetActive(true);
        for (int i = 0; i < _itemUIs.Length; i++)
        {
            //アイテムリストの外側を参照しようとしたら
            if (i >=  ItemList.Count)
            {
                Debug.Log("呼びました");
                _itemUIs[i].SetUI(Item.ItemSuper.Null);
            }
            else
            {
                _itemUIs[i].SetUI( ItemList[i]);
            }
        }
    }

}
