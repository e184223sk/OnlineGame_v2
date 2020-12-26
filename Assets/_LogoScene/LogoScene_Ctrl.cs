using UnityEngine; 
using UnityEngine.Video;

public class LogoScene_Ctrl : MonoBehaviour
{ 
    void Start()
    {
        Invoke("Next", (float)GetComponent<VideoPlayer>().clip.length + 3f);
    }

    void Next()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }
}
