
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
            tag = raw.Substring(0, raw.IndexOf(":"));
            data = raw.Substring(raw.IndexOf(":") + 1); 
        }
    }
    public Online_data() { }
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

        List<Online_data> xc = new List<Online_data>();

        if (optimisation)
        {
            for (var g = list.Count - 1; g >= 0; g--)
            {
                bool ccc = false;
                foreach (var f in xc.ToArray())
                    if (f.tag == list[g].tag)
                        ccc = true;
                    
                if (!ccc) xc.Add(list[g]);
            } 
        }  
        string datas = "";
        foreach (var c in xc.ToArray())
            datas +=  c.tag + ":" + c.data + "\n";
        NetData.server.FileUploadStr( path, datas);
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