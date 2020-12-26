using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class LogoScene_Ctrl : MonoBehaviour
{
    bool k; 
    public Image bg;

    void Start() => Invoke("Next", (float)GetComponent<VideoPlayer>().clip.length-0.3f);
    void Next() { k = true; }

    void Update()
    {
        if (k)
        { 
            bg.color = new Color(0, 0, 0, bg.color.a + Time.deltaTime);
            if (bg.color.a >= 2)
                SceneManager.LoadScene("TitleScene");
        }
    }

}
