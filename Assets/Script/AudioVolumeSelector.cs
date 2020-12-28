using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AudioVolumeSelector : MonoBehaviour
{
    public AudioType audioType;
    public bool StartTimeSettingOnly = true;

    void Start()
    {
        UPDATE();
        if (StartTimeSettingOnly)
            Destroy(this);
    }

    public void UPDATE()
    {
        switch (audioType)
        {
            case AudioType.None/*    */: GetComponent<AudioSource>().volume = AudioSystem.MASTER * AudioSystem.None; break;
            case AudioType.Bgm/*     */: GetComponent<AudioSource>().volume = AudioSystem.MASTER * AudioSystem.BGM; break;
            case AudioType.SoundEffect : GetComponent<AudioSource>().volume = AudioSystem.MASTER * AudioSystem.SE; break;
            case AudioType.Other/*   */: GetComponent<AudioSource>().volume = AudioSystem.MASTER * AudioSystem.OTHER; break;
            case AudioType.Voice/*   */: GetComponent<AudioSource>().volume = AudioSystem.MASTER * AudioSystem.VOICE; break;
        }
    }
}

public enum AudioType
{
    None,
    Bgm,
    SoundEffect,
    Other,
    Voice
}

public class AudioSystem : MonoBehaviour
{
    public static float BGM
    {
        get => bgm;
        set => bgm = value > 1 ? 1 : (value < 0 ? 0 : value);
    }
    public static float SE
    {
        get => se;
        set => se = value > 1 ? 1 : (value < 0 ? 0 : value);
    }
    public static float VOICE
    {
        get => voice;
        set => voice = value > 1 ? 1 : (value < 0 ? 0 : value);
    }

    public static float OTHER
    {
        get => sys;
        set => sys = value > 1 ? 1 : (value < 0 ? 0 : value);
    }


    public static float MASTER
    {
        get => master;
        set => master = value > 1 ? 1 : (value < 0 ? 0 : value);
    }

    static float bgm = None, se = None, voice = None, sys = None, master = None;
    public const float None = 0.7f;

#if UNITY_EDITOR
    [InitializeOnLoadMethod]
    private static void Editors_()
    {
        EditorApplication.update += () =>
        {
            if (EditorApplication.isPlaying) return;
            foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
            {
                // シーン上に存在するオブジェクトならば処理.
                if (obj.activeInHierarchy)
                {
                    var as_ = obj.GetComponent<AudioSource>();
                    if (as_ != null)
                    {
                        if (obj.GetComponent<AudioVolumeSelector>() == null)
                            obj.AddComponent<AudioVolumeSelector>();
                        if (obj.GetComponent<AudioVolumeSelector>().audioType == AudioType.None)
                        {
                            string path = obj.transform.gameObject.name;
                            Transform parent = obj.transform.parent;
                            while (parent != null)
                            {
                                path = parent.name + "/" + path;
                                parent = parent.parent;
                            }
                            Debug.LogError("オーディオタイプが設定されていません!! [AudioVolumeSelector] ::: Object Name (" + path + ")");
                        }
                    } 
                }
            }
        };
    } 
    //自動設置
#endif
 
}
