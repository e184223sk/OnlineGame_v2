using System.Collections.Generic;

public static class LogData_Manager 
{
    static List<Online_data> data = new List<Online_data>();
    public static void SAVE() => Online_data.SaveList(NetData.Get__LOG_TXT, data, false);
    public static void LOAD() => data = Online_data.GetList("log.txt");
    public static void ADD(Online_data c) => data.Add(c);
}
