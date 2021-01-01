
using System.Collections.Generic;

public class Online_data
{
    public string tag, data;
    public Online_data(string raw)
    {
        if (raw.IndexOf(":") < 0)
        {
            tag = raw;
            data = "";
        }
        else
        {
            tag = raw.Substring(0, raw.IndexOf(":") - 1);
            data = raw.Substring(raw.IndexOf(":") + 1);
        }
    }

    /// <summary>
    /// pathはPIN/以降だけでおｋ
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static List<Online_data> GetList(string path)
    {
        var data = new List<Online_data>();
        string raw = NetData.server.GetText(NetData.USERDIR + NetData.user.ID + "/" + NetData.user.PASS + "/" + path);
        string[] dataRaw = raw.Split(new string[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (var d in dataRaw)
            data.Add(new Online_data(d));
        return data;
    }



    public static void SaveList(string path, List<Online_data> list, bool optimisation = true)
    {
        if (optimisation) 
            for (var g = list.Count - 1; g >= 0; g--) 
                for (var cx = g; cx >= list.Count - 1; cx--) 
                    if (list[g].tag == list[cx].tag)
                        list.RemoveAt(cx);    
        string datas = "";
        foreach (var c in list.ToArray())
            datas =  c.tag + ":" + c.data + "\n";
        NetData.server.FileUploadStr(NetData.USERDIR + NetData.user.ID + "/" + NetData.user.PASS + "/" + path, datas);
    }

    /// <summary>
    /// pathはPIN/以降だけでおｋ
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetData(string tag ,List<Online_data> list, string NonFindTimeReturn = "[N/A]")
    {
        for (var g = list.Count - 1; g >= 0; g--)
            if (list[g].tag == tag)
                return list[g].data;
        return NonFindTimeReturn; 
    }
}