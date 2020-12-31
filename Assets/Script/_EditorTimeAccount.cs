using UnityEngine;
using UnityEngine.SceneManagement;

public class _EditorTimeAccount : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    static void INIT()
    {
        string c = SceneManager.GetActiveScene().name;
        if (Application.isEditor)
        {
            NetData.user.ID = "dev01a02b03c";
            NetData.user.PASS = "12851033";
            Debug.Log("[Set DevelopedAccount]\n    ID:dev01a02b03c\n    PIN:12851033");
        }
    }
}