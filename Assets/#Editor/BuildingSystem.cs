#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{ 
    const string root = "BUILD and RUN/";
    const string B = "ビルド";
    const string U = "アップロード";
    const string I = "インストーラを起動";
    const string R = "実行";

    [MenuItem(root + B)] static void T3() => TASK("b");
    [MenuItem(root + R)] static void T4() => TASK("r");
    [MenuItem(root + I)] static void TA() => TASK("i");
    [MenuItem(root + U)] static void TB() => TASK("p");

    [MenuItem(root + B + "→" + R)] static void T5() => TASK("br");
    [MenuItem(root + B + "→" + U)] static void T6() => TASK("bpr");
    [MenuItem(root + B + "→" + U + "→" + R)] static void T1() => TASK("bpr");
    [MenuItem(root + B + "→" + U + "→" + I)] static void T2() => TASK("bpi");
    [MenuItem(root + U + "→" + R)] static void T7() => TASK("bp");
    [MenuItem(root + U + "→" + I)] static void T9() => TASK("pi"); 
     
    static void TASK(string code)
    {
        foreach (var c in code) 
            switch (c)
            {
                case 'b': if (!Build()) E_LOG("ビルドに失敗しました"); break; 
                case 'r': if (!RunEXE(@"Build\System\OnlineGame.exe")) E_LOG("ゲームの実行に失敗しました"); break; 
                case 'p': if (!RunEXE(@"SubSystems\upload_BuildFile.exe")) E_LOG("アップローダの実行に失敗しました"); break;  
                case 'i': if (!RunEXE(@"SubSystems\installer.exe")) E_LOG("インストーラの実行に失敗しました"); break;
                default: E_LOG("存在しないタグです"); break;
            } 
    }

    static void E_LOG(string c) => UnityEngine.Debug.LogError(c);
   
    static bool Build()
    {
        EditorUtility.DisplayProgressBar("start", "", 0.1f);
        List<string> sceneList = new List<string>();
        EditorBuildSettingsScene[] xx = EditorBuildSettings.scenes;
        foreach (EditorBuildSettingsScene scene in xx)
        {
            if (scene.path.IndexOf("Debug") != -1) continue;
            sceneList.Add(scene.path);
        } 
        var scenes = sceneList.ToArray();

        EditorUtility.DisplayProgressBar("Settings", "", 0.3f);
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = "Build/System/OnlineGame.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;

        EditorUtility.DisplayProgressBar("Build", "", 0.5f );

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);

        EditorUtility.DisplayProgressBar("Build", "", 0.9f);
        EditorUtility.ClearProgressBar();
        return report.summary.result == BuildResult.Succeeded; 
    }
     
    static bool RunEXE(string c)
    {
        string ROOTDIR = Directory.GetParent(Application.dataPath.Replace("/", @"\")) + @"\";
        if (!File.Exists(ROOTDIR + c)) return false;
        Process.Start(ROOTDIR + c);
        return true;
    }

}


#endif