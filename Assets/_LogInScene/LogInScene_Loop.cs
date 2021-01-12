using UnityEngine;
using UnityEngine.UI;

public class LogInScene_Loop : MonoBehaviour
{

    [SerializeField]
    RawImage cover;

    bool isset;
    Color col;

    [SerializeField]
    RawImage ring;

    [SerializeField]
    Text text;
      
    public void NORMAL(bool c)
    { 
        isset = false;
        Invoke(c ? "CLEAR" : "MISS", 5);
        cover.enabled = c;
    }

    void CLEAR()
    {
        isset = true;
        col = Color.green;
        Invoke("DOWN", 0.5f); 
    }

    void MISS()
    {
        isset = true;
        col = Color.red;
        Invoke("DOWN", 0.5f);
    }

    void DOWN()
    {
        gameObject.active = false;
    }

    void Update()
    {
        if (isset)
        {
            text.text = "";
            ring.color = col;
        }
        else
        {
            text.text = "アカウント情報を確認中" + "".PadLeft(((int)(Time.time / 2)) % 4, '.');
            ring.transform.Rotate(Vector3.forward * Time.deltaTime * -360);
            ring.color = Color.HSVToRGB(Time.time % 1f, 0.5f, 1);
        }
    }
}
