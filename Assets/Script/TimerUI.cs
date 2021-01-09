using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    void Awake() => time = time;

    float _time;
    [SerializeField] RawImage d3, d2, d1, d0, f0, f1, f2;
    [SerializeField] Texture2D[] num;
    
    public float time
    {
        get { return _time; }
        set
        {
            float ttt = _time;
            if (0 > value) _time = 0;
            else if (6000 < value) _time = 99 * 60 + 59.999f;
            else _time = value;
            if (ttt != _time)
            {
                int min = (int)(_time / 60);
                int sec = (int)(_time % 60);
                int ms_ = (int)(_time % 1 * 1000);

                d3.texture = num[min / 10];
                d2.texture = num[min % 10];
                d1.texture = num[sec / 10];
                d0.texture = num[sec % 10];
                f0.texture = num[ms_ / 100];
                f1.texture = num[ms_ / 10%10];
                f2.texture = num[ms_ % 10];
            }
        }
    }

}
