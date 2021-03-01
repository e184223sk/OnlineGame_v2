using MonobitEngine;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Rendering;

public class ChatScript : MonobitEngine.MonoBehaviour
{
    #region Private Field

    //プレイヤー名
    string _playerName = "test";

    //送信する文字を入れるフィールド
    Text _receiveText;

    //受信する文字を入れるフィールド
    InputField _sendField;

    private GameObject _test;

    #endregion

    #region SerializeField

    //MonobitViewコンポーネント
    [SerializeField]
    MonobitView _monobitview;


    //送信する文字を入れるGameObject
    [SerializeField]
    GameObject _sendObject;

    //受信する文字を入れるGameObject
    [SerializeField]
    GameObject _receiveObject;


    #endregion

    #region private parameter
    private string[] _messages = new string[]
    {
        "こんにちは",
        "Hello",
        "ニィハオ"
    };

    private int[] _nums = new int[]
    {
        30000,
        54,
        5431,
        7894
    };
    #endregion

    #region MUN CallBacks
    [MunRPC]
    void ReceiveMessage(string message)
    {
        Debug.Log("受信");

        _receiveText.text = message;
    }

    [MunRPC]
    void ReceiveMessage(string playerName, string message)
    {
        //自分が送ったチャットは赤字で表示
        _receiveText.text += (playerName == MonobitNetwork.player.name) ?
            Environment.NewLine + "<color=red>" + playerName + " : " + message +  "</color>" :
            Environment.NewLine + "<color=black>" + playerName + " : " + message + "</color>";

    }
    [MunRPC]
    void ReceiveMessages(string[] i)
    {
        foreach (var ii in i)
            _receiveText.text += ii;
    }

    [MunRPC]
    void ReceiveTransform(object obj)
    {
        Debug.Log(obj);
    }

    #endregion


    #region Unity CallBacks

    void Awake()
    {
        // すべての親オブジェクトに対して MonobitView コンポーネントを検索する
        if (GetComponentInParent<MonobitView>() != null)
        {
            _monobitview = GetComponentInParent<MonobitView>();
        }
        // 親オブジェクトに存在しない場合、すべての子オブジェクトに対して MonobitView コンポーネントを検索する
        else if (GetComponentInChildren<MonobitView>() != null)
        {
            _monobitview = GetComponentInChildren<MonobitView>();
        }
        // 親子オブジェクトに存在しない場合、自身のオブジェクトに対して MonobitView コンポーネントを検索して設定する
        else
        {
            _monobitview = GetComponent<MonobitView>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        _test = GameObject.Find("left");

        //各種コンポーネントの取得------------------------------------
        
        _sendField = _sendObject.GetComponent<InputField>();
        _receiveText = _receiveObject.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        _sendField.onEndEdit.AddListener(SendChat);

        //チャット欄を非表示------------------------------------------
        _receiveObject.SetActive(false);
        _sendObject.SetActive(false);

        //------------------------------------------------------------

    }



    // Update is called once per frame
    void Update()
    {
        //チャットモードの切り替え---------------------------
        if (MonobitNetwork.inRoom)
        {
            //ReceiveFieldは部屋に入っているときは終始表示
            if(!_receiveObject.activeSelf)
                _receiveObject.SetActive(true);
            if (!NetworkCtrler._InPlay)
            {
                _sendObject.SetActive(true);
                _sendField.ActivateInputField();
            }
            else
            {
                _sendObject.SetActive(false);
            }
        }
        //-------------------------------------------------

        //チャットモードの時　F1　で受信フィールドのテキストクリア-------------------------
        
        if (!NetworkCtrler._InPlay && Input.GetKeyDown(KeyCode.F1))
            ClearReceive();

        if (!NetworkCtrler._InPlay && Input.GetKeyDown(KeyCode.F2))
        {
            SendTransform();
        }

        //----------------------------------------------------------------------------------------

    }
    #endregion

    #region private Methods
    /// <summary>
    /// チャット入力欄のテキストを全員に送信する関数
    /// </summary>
    public void SendChat(string text)
    {
        if (String.IsNullOrEmpty(_sendField.text))
            return;

        //不適切な表現を削除--------------------------------------------------------------

        text = _sendField.text;
        text = text.Replace(" ", "").Replace("　", "").Trim();
        text = ForbiddenWords.WordFilter(text);
        ClearSend();
        _sendField.ActivateInputField();
        if (text == "")
        {
            _receiveText.text += Environment.NewLine + "<color=red>不適切な表現を含むため送信できません</color>";
            _sendObject.SetActive(false);
            return;
        }

        //---------------------------------------------------------------------------------


        //送信
        _monobitview.RPC("ReceiveMessage", MonobitTargets.All, MonobitNetwork.player.name, text);
        NetworkCtrler._InPlay = true;

    }


    private void SendMessage(string text)
    {
        _monobitview.RPC("ReceiveMessage", MonobitTargets.All, text);
    }
    private void SendMessages()
    {
        _monobitview.RPC("ReceiveMessages", MonobitTargets.All, _messages);
    }

    private void SendTransform()
    {
        //_monobitview.RPC("ReceiveTransform", MonobitTargets.All, _test.transform.rotation);
        //_monobitview.RPC("ReceiveTransform", MonobitTargets.All, );
    }

    #endregion


    #region Public Methods

    public void ClearSend()
    {
        _sendField.text = "";
    }

    public void ClearReceive()
    {
        _receiveText.text = "";
    }

    #endregion

}



