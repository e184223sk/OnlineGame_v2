using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
 
public class Refresh_cacheError : EditorWindow
{
    [MenuItem(@"RefreshError/Library\PackageCache\com.unity.collab-proxy@")]
    private static void Create()
    {
        string c = Directory.GetParent(Application.dataPath).ToString() + @"\Library\PackageCache\com.unity.";
        Delete(c + "collab-proxy@1.3.9");
        Delete(c + "textmeshpro@3.0.1"); 
    }

    public static void Delete(string targetDirectoryPath)
    {
        if (!Directory.Exists(targetDirectoryPath))
        {
            return;
        }

        //ディレクトリ以外の全ファイルを削除
        string[] filePaths = Directory.GetFiles(targetDirectoryPath);
        foreach (string filePath in filePaths)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            File.Delete(filePath);
        }

        //ディレクトリの中のディレクトリも再帰的に削除
        string[] directoryPaths = Directory.GetDirectories(targetDirectoryPath);
        foreach (string directoryPath in directoryPaths)
        {
            Delete(directoryPath);
        }

        //中が空になったらディレクトリ自身も削除
        Directory.Delete(targetDirectoryPath, false);
    }
}