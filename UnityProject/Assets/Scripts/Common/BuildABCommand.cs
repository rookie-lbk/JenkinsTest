using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildABCommand
{

    private const string BaseWorkPath = "WorkAssets/";
    private static IDictionary<string, List<string>> packDir = new Dictionary<string, List<string>>()
    {
        {
            "*.prefab",new List<string>()
            {
                BaseWorkPath + "prefab",
            }
        },
        {
            "*.png",new List<string>()
            {
                BaseWorkPath + "textures",
            }
        },
        {
            "*.mat",new List<string>()
            {
                BaseWorkPath + "materials",
            }
        }
    };

    public static AssetBundleBuild[] OnStartBuildAB()
    {
        var startTime = Environment.TickCount;
        BuildTarget buildTarget = BuildTarget.Android;

        // Dictionary<string, bool> allNeedFiles = new Dictionary<string, bool>(1024 * 64);
        // foreach (var item in packDir)
        // {
        //     string endSuffix = item.Key;
        //     List<string> directoryList = item.Value;
        //     foreach (var directoryPath in directoryList)
        //     {
        //         foreach (var path in Directory.GetFiles(Path.Combine(Application.dataPath, directoryPath), endSuffix, SearchOption.AllDirectories))
        //         {
        //             allNeedFiles.Add(path, true);
        //         }
        //     }
        // }

        AssetBundleBuild[] data = new AssetBundleBuild[2];
        for (int i = 0; i < data.Length; i++)
        {
            AssetBundleBuild assetBundleBuild = new AssetBundleBuild();
            assetBundleBuild.assetBundleName = i + ".ab";
            List<string> items = new List<string>();
            items.Add("WorkAssets/textures/" + i + ".png");
            assetBundleBuild.assetNames = items.ToArray();
            data[i] = assetBundleBuild;
            Debug.Log(assetBundleBuild.assetBundleName + " " + assetBundleBuild.assetNames[0]);
        }

        ClearAssetBundlesName();
        AssetDatabase.Refresh();
        var options = BuildAssetBundleOptions.ChunkBasedCompression
            | BuildAssetBundleOptions.StrictMode
            //| BuildAssetBundleOptions.DisableWriteTypeTree
            | BuildAssetBundleOptions.DeterministicAssetBundle;
        Debug.Log(Application.streamingAssetsPath);
        AssetBundleManifest result = BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/Resources_AB", data, options, buildTarget);
        if (!result)
        {
            Debug.Log(result);
            throw new Exception(string.Format("MakeAssetBundles Error {0}", result));
        }

        Debug.Log("OnStartBuildAB OVER use time " + (Environment.TickCount - startTime));
        return data;
    }



    public static void ClearAssetBundlesName()
    {
        int length = AssetDatabase.GetAllAssetBundleNames().Length;
        string[] oldAssetBundleNames = new string[length];
        for (int i = 0; i < length; i++)
        {
            oldAssetBundleNames[i] = AssetDatabase.GetAllAssetBundleNames()[i];
        }
        for (int j = 0; j < oldAssetBundleNames.Length; j++)
        {
            AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[j], true);
        }
    }
}
