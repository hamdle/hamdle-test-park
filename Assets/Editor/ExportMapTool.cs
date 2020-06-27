using System;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

/// <summary>
/// One-click export of the active scene to an asset bundle
/// Asset bundle is exporter to the project root folder and copied into the Skater XL maps folder in MyDocuments
/// </summary>
public static class ExportMapTool
{
    private const string ASSET_BUNDLES_BUILD_PATH = "AssetBundles";

    [MenuItem("SXL/Quick Map Export")]
    public static void ExportMap()
    {
        var scene = SceneManager.GetActiveScene();

        EditorSceneManager.SaveScene(scene);

        var bundle_name = scene.name;

        var build = new AssetBundleBuild
        {
            assetBundleName = bundle_name,
            assetNames = new[] {scene.path}
        };

        if (!Directory.Exists(ASSET_BUNDLES_BUILD_PATH))
            Directory.CreateDirectory(ASSET_BUNDLES_BUILD_PATH);

        BuildPipeline.BuildAssetBundles(ASSET_BUNDLES_BUILD_PATH, new []{ build }, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

        var map_dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SkaterXL/Maps");
        var bundle_path = Path.Combine(Application.dataPath.Replace("/Assets", "/AssetBundles"), build.assetBundleName);
        var dest_path = Path.Combine(map_dir, build.assetBundleName);
      
        Debug.Log($"Copying {bundle_path} to {dest_path}");

        File.Copy(bundle_path, dest_path, overwrite: true);
        File.Delete(bundle_path);

        EditorSceneManager.OpenScene(scene.path);
    }
}