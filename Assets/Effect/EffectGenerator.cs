using UnityEngine;

public class EffectGenerator : MonoBehaviour
{
    public static GameObject CreateEffect(EffectType type, Vector3 position, Quaternion rotation, float LifeTime)
    {
        if (LifeTime < 0)
        {
            switch (type)
            {
                case EffectType.Splash          : LifeTime = 05.00f; break;
                case EffectType.Smoke           : LifeTime = 12.00f; break; 
                case EffectType.LargeBlood      : LifeTime = 03.00f; break;
                case EffectType.SlashBlood      : LifeTime = 03.00f; break;
                case EffectType.GunDamageBlood  : LifeTime = 03.00f; break;
                case EffectType.Explosion_1x1   : LifeTime = 10.00f; break;
                case EffectType.Explosion_3x3   : LifeTime = 10.00f; break;
                case EffectType.Explosion_10x10 : LifeTime = 10.00f; break; 
                case EffectType.GunFire         : LifeTime = 00.50f; break;
                case EffectType.LargeFire       : LifeTime = 12.00f; break;
                case EffectType.NarrowFire      : LifeTime = 12.00f; break;
                case EffectType.Burner          : LifeTime = 12.00f; break;
                case EffectType.ExhaustGas      : LifeTime = 12.00f; break;
                case EffectType.Explosion_40x40 : LifeTime = 12.00f; break;
                case EffectType.GetItems        : LifeTime = 04.00f; break;
                default : print("このエフェクトタイプはライフタイムの自動割り当てが定義されていません"); break;
            }
        }
        var v = (Instantiate(Resources.Load("effect/" + type.ToString()), position, rotation) as GameObject);
        v.GetComponent<EffectLimitter>().SetLifeTime(LifeTime);
        v.GetComponent<ParticleSystem>().Play();
        return v;
    }

    //OverLoad---------------------------------------------------------------------------------------------------------------------------------------------------
    public static GameObject CreateEffect(EffectType type, Vector3 position, Quaternion rotation)   => CreateEffect(type, position, rotation, -1);
    public static GameObject CreateEffect(EffectType type, Vector3 position)                        => CreateEffect(type, position, Quaternion.identity, -1);
    public static GameObject CreateEffect(EffectType type, Vector3 position, float LifeTime)        => CreateEffect(type, position, Quaternion.identity, LifeTime);
}

/// <summary>
/// エフェクトの種類
/// </summary>
public enum EffectType
{
    /// <summary>
    /// 広範囲の血
    /// </summary>
    LargeBlood,

    /// <summary>
    /// 斬撃ダメージの血
    /// </summary>
    SlashBlood,

    /// <summary>
    /// 銃撃ダメージの血
    /// </summary>       
    GunDamageBlood,
    
    /// <summary>
    /// 銃の発射エフェクト
    /// </summary>
    GunFire,

    /// <summary>
    /// 広範囲の火災
    /// </summary>
    LargeFire,

    /// <summary>
    /// 狭範囲の火災
    /// </summary>
    NarrowFire,

    /// <summary>
    /// バーナー的な炎
    /// </summary>
    Burner,

    /// <summary>
    /// 廃棄ガス
    /// </summary>
    ExhaustGas, 
    
    /// <summary>
    /// スモークグレネード
    /// </summary>
    Smoke,
             

    /// <summary>
    /// 1Mくらいの爆発
    /// </summary>
    Explosion_1x1,

    /// <summary>
    /// 3Mくらいの爆発
    /// </summary>

    Explosion_3x3,

    /// <summary>
    /// 10Mくらいの爆発
    /// </summary>
    Explosion_10x10,
     
    /// <summary>
    /// 巨大な爆発
    /// </summary>
    Explosion_40x40,
    
    /// <summary>
    /// 水しぶき
    /// </summary>
    Splash,     

    /// <summary>
    /// アイテム取得時のエフェクト
    /// </summary>
    GetItems,            
}

 