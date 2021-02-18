using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using UnityEngine.UI;
using System.Threading;
using UnityEngine.SceneManagement;

public class NetworkCtrler : MonobitEngine.MonoBehaviour
{
    #region Private Fields

    //プレイヤー系-----------------------------------------------------
    //ユニティちゃんPrefab
    [SerializeField]
    private GameObject _player;


    //プレイヤー名
    string _playerName = "";

    //----------------------------------------------------------------

    //ネットワーク関連のクラス
    MonobitView _monobitView;

    //カメラ関係--------------------------------------------------------
    //カメラ制御
    CameraCtrl _cameraCtrl;

    //プレイヤーがいないときのカメラ注視点
    Transform _defaultTarget;

    //----------------------------------------------------------------

    //入室関係-------------------------------------------------------
    //名前入力欄オブジェクト
    GameObject _nameObject;

    //退室ボタンオブジェクト
    GameObject _leaveObject;
    
    //入室ボタンオブジェクト
    GameObject _enterObject;

    //名前入力欄
    InputField _nameField;
    
    //入室ボタン
    Button _enterBtn;

    //退室ボタン
    Button _leaveBtn;

    //----------------------------------------------------------------

    //ステージギミック
    [SerializeField]
    private GameObject _gimmick;

    #endregion


    #region Static Field
    //ゲームプレイモードとチャットモードを切り替えるための変数    true = ゲームプレイモード false = チャットモード
    public static bool _InPlay = true;

    public static MonobitView _view;

    #endregion

    #region MUN RPC
    [MunRPC]
    private void LoadStageRequest()
    {
        Debug.Log("全員に送信！");
        _monobitView.RPC("LoadStage",
            MonobitTargets.All,
            _gimmick.transform.position,
            _gimmick.transform.rotation,
            _gimmick.transform.localScale);
    }


    [MunRPC]
    private void LoadStage(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Debug.Log("position : " + position);
        Debug.Log("rotation : " + rotation);
        Debug.Log("sclae    : " + scale);

        _gimmick.transform.position = position;
        _gimmick.transform.rotation = rotation;
        _gimmick.transform.localScale = scale;

    }

    #endregion

    #region Unity CallBacks

    // Start is called before the first frame update
    void Start()
    {
        //起動時のウィンドウサイズを(500,500)に設定　　>>正直このスクリプトでやる意味ない
        /*Screen.SetResolution(500, 500, false);

        //入室関係-------------------------------------------------------
        //InputField取得
        _nameObject = GameObject.Find("NameField");
        _nameField = _nameObject.GetComponent<InputField>();
        _nameField.ActivateInputField();
        _nameField.onEndEdit.AddListener(EnterRoom);

        //ボタン取得
        _enterObject = GameObject.Find("EnterButton");
        _leaveObject = GameObject.Find("LeaveButton");
        _enterBtn = _enterObject.GetComponent<Button>();
        _leaveBtn = _leaveObject.GetComponent<Button>();

        _leaveObject.SetActive(false);

        //イベント設定
        _enterBtn.onClick.AddListener(EnterRoom);
        _leaveBtn.onClick.AddListener(LeaveRoom);


        //---------------------------------------------------------------

        //カメラ関係----------------------------------------------
        _defaultTarget = GameObject.Find("front").transform;            //カメラ注視オブジェクト
        _cameraCtrl = Camera.main.gameObject.GetComponent<CameraCtrl>();//コンポーネント
        _cameraCtrl.target = _defaultTarget;                            //注視点セット
        */
        //monobitview 取得
        _monobitView = this.gameObject.GetComponent<MonobitView>();


        //ギミック取得
        //_gimmick = GameObject.Find("Gimmick");


        _view = _monobitView;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.F3) && !MonobitNetwork.isHost)
        {
            _monobitView.RPC("LoadStageRequest", MonobitTargets.Host);
        }

        //エスケープキーでゲームとチャットを切り替え
        if (Input.GetKeyDown(KeyCode.Escape))
            _InPlay = !_InPlay;
        */
        //サーバーに未接続のとき自動で接続
        if (!MonobitNetwork.isConnect)
        {
            MonobitNetwork.autoJoinLobby = true;
            MonobitNetwork.ConnectServer("SimpleServer");
        }
        else
        {
            MonobitNetwork.CreateRoom("test");
            //部屋に入ったときにプレイヤーを生成
            /*if (MonobitNetwork.inRoom && _player == null)
            {
                _player = MonobitNetwork.Instantiate("unitychan 1", Vector3.zero, Quaternion.identity, 0);
                _player.transform.position = new Vector3(Random.Range(10, 20), 1f, Random.Range(-15, 20));
                _cameraCtrl.target = _player.transform;
            }*/
        }


    }
    
    #endregion

    #region Public Methods

    /// <summary>
    /// 入室機能
    /// </summary>
    /// <param name="name">プレイヤー名</param>
    public void EnterRoom(string name)
    {
        //プレイヤー名を入力
        _playerName = name;


        //プレイヤー名が入力されていれば部屋への入室するボタン  
        if ( !string.IsNullOrEmpty(_playerName))
        {
            if (!MonobitNetwork.inRoom)
            {
                //サーバ上の部屋を全取得
                RoomData[] _roomDatas = MonobitNetwork.GetRoomData();
                //部屋が1つでもあるなら
                if (_roomDatas.Length >= 1)
                {
                    //1つ目の部屋に入る
                    MonobitNetwork.JoinRoom(_roomDatas[0].name);
                    
                    //名前入力欄隠す
                    _nameObject.SetActive(false);
                    _enterObject.SetActive(false);
                    
                    //退出ボタン表示
                    _leaveObject.SetActive(true);

                }
                else
                {
                    //「SimpleGame」という名前で部屋を作る
                    MonobitNetwork.CreateRoom("SimpleGame");
                    
                    //名前入力欄隠す
                    _nameObject.SetActive(false);
                    _enterObject.SetActive(false);
                    
                    //退出ボタン表示
                    _leaveObject.SetActive(true);


                }
                MonobitNetwork.player.name = _playerName;
            }
        }
    }

    /// <summary>
    /// 入室機能
    /// </summary>
    public void EnterRoom()
    {

        if (!MonobitNetwork.inLobby)
            return;

        //プレイヤー名を入力
        _playerName = _nameField.text;


        //プレイヤー名が入力されていれば部屋への入室するボタン  
        if (!string.IsNullOrEmpty(_playerName))
        {
            if (!MonobitNetwork.inRoom)
            {
                //サーバ上の部屋を全取得
                RoomData[] _roomDatas = MonobitNetwork.GetRoomData();
                //部屋が1つでもあるなら
                if (_roomDatas.Length >= 1)
                {
                    //1つ目の部屋に入る
                    MonobitNetwork.JoinRoom(_roomDatas[0].name);

                    //名前入力欄隠す
                    _nameObject.SetActive(false);
                    _enterObject.SetActive(false);
                    _leaveObject.SetActive(true);
                }
                else
                {
                    Debug.Log("部屋作る");
                    //「SimpleGame」という名前で部屋を作る
                    MonobitNetwork.CreateRoom("SimpleGame");

                    //名前入力欄隠す
                    _nameObject.SetActive(false);
                    _enterObject.SetActive(false);
                    _leaveObject.SetActive(true);
                }
                MonobitNetwork.player.name = _playerName;
            }
        }
    }
    public void EnterandmoveRoom()
    {
        //プレイヤー名を入力
        _playerName = _nameField.text;


        //プレイヤー名が入力されていれば部屋への入室するボタン  
        if (!string.IsNullOrEmpty(_playerName))
        {
            if (!MonobitNetwork.inRoom)
            {
                //サーバ上の部屋を全取得
                RoomData[] _roomDatas = MonobitNetwork.GetRoomData();
                //部屋が1つでもあるなら
                if (_roomDatas.Length >= 1)
                {
                    //1つ目の部屋に入る
                    MonobitNetwork.JoinRoom(_roomDatas[0].name);

                    //名前入力欄隠す
                    _nameObject.SetActive(false);
                    _enterObject.SetActive(false);
                    SceneManager.LoadScene("BirthdayScene");
                }
                else
                {
                    //「SimpleGame」という名前で部屋を作る
                    MonobitNetwork.CreateRoom("SimpleGame");

                    //名前入力欄隠す
                    _nameObject.SetActive(false);
                    _enterObject.SetActive(false);
                    SceneManager.LoadScene("BirthdayScene");
                }
                MonobitNetwork.player.name = _playerName;
            }
        }
    }


    /// <summary>
    /// 退出機能
    /// </summary>
    public void LeaveRoom()
    {
        if (MonobitNetwork.inRoom)
        {
            _cameraCtrl.target = _defaultTarget;
            Destroy(_player, 1f);
            MonobitNetwork.LeaveRoom();

            //名前入力欄表示
            _nameObject.SetActive(true);
            _enterObject.SetActive(true);

            //退出ボタン非表示
            _leaveObject.SetActive(false);
        }
    }
    public void LeaveandMoveRoom()
    {
        if (MonobitNetwork.inRoom)
        {
            _cameraCtrl.target = _defaultTarget;
            Destroy(_player, 1f);
            MonobitNetwork.LeaveRoom();

            //名前入力欄表示
            _nameObject.SetActive(true);
            _enterObject.SetActive(true);

            //退出ボタン非表示
            _leaveObject.SetActive(false);
            SceneManager.LoadScene("TitleScene");

        }
    }

    #endregion

}
