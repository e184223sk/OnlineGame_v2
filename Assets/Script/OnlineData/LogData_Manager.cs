using System.Collections.Generic;

public static class LogData_Manager 
{
    static List<Online_data> data = new List<Online_data>();
    public static void SAVE() => Online_data.SaveList("log.txt", data, false);
    public static void LOAD() => data = Online_data.GetList("log.txt");
}
