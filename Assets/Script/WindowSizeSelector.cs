using UnityEngine;

public class WindowSizeSelector : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    static void INIT() => new GameObject("[WindowSizeHundle]").AddComponent<WindowSizeSelector>().Set();

    public void Set()  
    {
        DontDestroyOnLoad(gameObject);
        if(Mathf.Abs(Screen.height * 1f / Screen.width - 0.75f)>0.1f)
            SetWindow(Screen.fullScreen);
    }
    
    void Update()
    { 
       if(Key.FL.Down) SetWindow(!Screen.fullScreen);
    }

    void SetWindow(bool inve)
    {
        int g = Screen.currentResolution.width * 2 / 3;
        Screen.SetResolution(g, g * 3 / 4,inve);
    }
}
