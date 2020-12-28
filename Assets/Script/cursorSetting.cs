using UnityEngine;

public class cursorSetting
{
    [RuntimeInitializeOnLoadMethod]
    static void _Start() => Cursor.visible = false;
}