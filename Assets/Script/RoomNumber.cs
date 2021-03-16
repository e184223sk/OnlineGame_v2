public  static class RoomNumber
{
    /// <summary>
    /// 部屋番
    /// </summary>
    public static int NUMBER
    {
        get => number;
        set => number = value > 63 ? 63 : value < 0 ? 0 : value;
    }

    /// <summary>
    /// ゲーム終了後　部屋番をリセット
    /// </summary>
    public static void ResetNumber() => number = -1;

    static int number;
}
