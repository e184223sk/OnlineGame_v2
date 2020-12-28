using UnityEngine; 
using UnityEngine.Video; 

public class LogoScene_Ctrl : MonoBehaviour
{ 
    void Start() => Invoke("Next", (float)GetComponent<VideoPlayer>().clip.length-0.3f);
    void Next()  => SceneLoader.Load("TitleScene");
}
