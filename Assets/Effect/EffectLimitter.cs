//時間経過でエフェクトのオブジェクトを破棄するシステム
public class EffectLimitter : UnityEngine.MonoBehaviour
{
    public void SetLifeTime(float p)
    {
        if (p > 0)
            Destroy(gameObject, p);
    }
}