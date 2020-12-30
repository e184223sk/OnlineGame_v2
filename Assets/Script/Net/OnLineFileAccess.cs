using UnityEngine;

public class OnLineFileAccess
{
    public static Texture   GetTexture(string url) { return Get(url).texture as Texture; }
    public static Texture2D GetTexture2D(string url) { return Get(url).texture; }
    public static string GetText(string url) { return Get(url).text; }
    public static byte[] GetBytes(string url) { return Get(url).bytes; }

    static WWW Get(string url)
    { 
        WWW w= new WWW(url); 
        while (!w.isDone) ;
        return w;
    }

}
