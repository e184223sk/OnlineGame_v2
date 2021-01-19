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
    MonobitView _monobitView;


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
        _monobitView.RPC("ReceiveMessage", MonobitTargets.All, MonobitNetwork.player.name, text);
        NetworkCtrler._InPlay = true;

    }


    private void SendMessage(string text)
    {
        _monobitView.RPC("ReceiveMessage", MonobitTargets.All, text);
    }
    private void SendMessages()
    {
        _monobitView.RPC("ReceiveMessages", MonobitTargets.All, _messages);
    }

    private void SendTransform()
    {
        //_monobitView.RPC("ReceiveTransform", MonobitTargets.All, _test.transform.rotation);
        //_monobitView.RPC("ReceiveTransform", MonobitTargets.All, );
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

//不適切な表現を規制するクラス
public class ForbiddenWords
{
    #region Static Fields
    static readonly string[] Words = new string[]{
"青姦","あおかん",
"アメ公","あめこう",
"アルコール依存症",
"犬殺し","いぬごろし",
"淫売","いんばい","売春",
"うんこ","うんこ",
"うんち",
"穢多","えた",
"ガキ","がき",
"皮被り","かわかぶり","包茎",
"姦通","かんつう",
"キ印","きじるし","精神障害者",
"キチ","きち",
"気違い",
"屑屋","くず","クズ",
"くわえ込む","くわえこむ",
"クンニ","くんに",
"強姦","ごうかん",
"ゴミ","ごみ",
"千摺り","せんずり","オナニー",
"ちんこ","チンコ","ちんぽ","チンポ","ちんちん","チンチン","ソープ",
"非人","ひにん",
"ブス","ぶす",
"部落","ぶらく",
"マンコ","まんこ","女性器","ほーみ","べちょこ","おめこ",
"セックス","せっくす"
,"あなる","アナル",
"おっぱい","オッパイ",
"死ね",
"ガイジ","がいじ",
"エロ","インポ","いんぽ","陰毛","淫乱","えっち","エッチ","おしり","顔射","がんしゃ","射精","スケベ","スカトロ","絶倫","前立腺","ちぇりーぼーい","チェリーボーイ","童貞","どうてい","てまん","手マン","なかだし","膣","発情","フェラ","ホモ","ほも","マリファナ","レイプ","性交",
"シコシコ",
"fuck","Fuck",
"shit","Shit",
"Suck","suck",
"faggot","Faggot",
"Nigger","Negro","Nigga","nigger","negro","nigga",
"sex","Sex","SEX","piss","dick","Dick","cunt","Cunt","tits","Tits",
"ass","Ass","prick","1919","4545","0721"

    };

    #endregion

    public static string WordFilter(string text)
    {
        foreach (string s in Words)
        {
            if (text.Contains(s))
            {
                text = "";
            }
        }

        return text;
    }

}



