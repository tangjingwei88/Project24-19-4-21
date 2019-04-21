//using UnityEngine;
//using System.Collections;
//using UnityEditor;
//using System.IO;

//public class AssetBundleBuilder : Editor {

//    //BuildAssetBundleOptions.ChunkBasedCompression:Assetbundle 的压缩格式为lz4，默认的是lzma格式，下载Assetbundle后立即解压，
//    //而lz4格式的Assetbundle会在加载资源的时候才进行解压，只是解压的资源的时机不一样
//    private static BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression;

//    private static int languageType = 0;  //0ch,1ct

//    [MenuItem("AssetBundles/ChangeToResourceLoadMode", false, 1)]
//    public static void ChangeToResourceLoadMode()
//    {
//        MoveFilesToResourceMode();
//        ModifyGameDefineAssetBundleMode(false);
//    }


//    [MenuItem("AssetBundles/ChangeToAssetBundleMode", false, 2)]
//    public static void ChangeToAssetBundleMode()
//    {
//        MoveFilesToAssetBundleMode();
//        ModifyGameDefineAssetBundleMode(true);
//    }



//    private static void ModifyGameDefineAssetBundleMode(bool assetbundleMode)
//    {
//        string filePath = Application.dataPath + "/Scripts/GameSetting/GameDefine.cs";
//        string s = File.ReadAllText(filePath);

//        if (assetbundleMode)
//            s = s.Replace("public static bool assetbundleMode = false;", "public static bool assetbundleMode = true;");
//        else
//            s = s.Replace("public static bool assetbundleMode = true;", "public static bool assetbundleMode = false;");

//        File.WriteAllText(filePath,s);

//        AssetDatabase.Refresh();

//    }

//    private static void MoveFilesToResourceMode()
//    {
//        DoMoveYeah(true);
//    }


//    private static void MoveFilesToAssetBundleMode()
//    {
//        DoMoveYeah(false);
//    }


//    private static void DoMoveYeah(bool revert)
//    {
//        MoveDirectory("/Resource/CharacterPrefab","/CharacterPrefab",revert);
//        MoveDirectory("/Resource/Characters","/Characters",revert);
//        MoveDirectory("/Resource/Config","/ABundles/Config",revert);

//        MoveDirectory("/Resource/UIPrefab","/UIPrefab",revert);

//        AssetDatabase.SaveAssets();
//    }

//    ///移动一个目录下的所有文件
//    private static void MoveDirectory(string fromPath, string toPath, bool revert = false)
//    {
//        if (revert)
//        {
//            string temp = fromPath;
//            fromPath = toPath;
//            toPath = temp;
//        }

//        string fullFromPath = Application.dataPath + fromPath;
//        string fullToPath = Application.dataPath + toPath;

//        DirectoryInfo fromDirInfo = new DirectoryInfo(fullFromPath);
//        DirectoryInfo toDirInfo = new DirectoryInfo(fullToPath);

//        if (!fromDirInfo.Exists)
//        {
//            Debug.LogErrorFormat("path {0} not found!", fullFromPath);
//            return;
//        }

//        FileInfo[] fis = fromDirInfo.GetFiles("*.*",SearchOption.AllDirectories);
//        //该目录下的文件总数
//        int totalCount = fis.Length;
//        int count = 0;
//        foreach (var item in fis)
//        {
//            count++;

//            if (EditorUtility.DisplayCancelableProgressBar(string.Format("从{0}移动文件到{1}", fromPath, toPath), item.Name, count * 1.0f / totalCount))
//                break;

//            //.meta文件直接跳过
//            if (item.Extension == ".meta") continue;

//            string restPath = item.FullName.Substring(fullFromPath.Length).Replace("\\","/");
//            //移动该文件
//            MoveOneFile(fromPath + restPath,toPath + restPath);
//        }
//        EditorUtility.ClearProgressBar();

//        AssetDatabase.Refresh();
//    }

//    //移动一个文件
//    private static void MoveOneFile(string fromPath,string toPath,bool revert = false)
//    {
//        if (revert)
//        {
//            string s = fromPath;
//            fromPath = toPath;
//            toPath = s;
//        }

//        string fullFromPath = Application.dataPath + fromPath;
//        string fullToPath = Application.dataPath + toPath;

//        FileInfo fromFile = new FileInfo(fullFromPath);
//        FileInfo toFile = new FileInfo(fullToPath);

//        if (fromFile.Extension == ".meta") return;

//        if (!fromFile.Directory.Exists || !fromFile.Exists)
//        {
//            Debug.LogErrorFormat("file {0} not found!",fullFromPath);
//            return;
//        }

//        if (!toFile.Directory.Exists)
//        {
//            Directory.CreateDirectory(toFile.DirectoryName);
//            AssetDatabase.Refresh();
//        }

//        //移动文件
//        string errorStr = AssetDatabase.MoveAsset("Assets" + fromPath ,"Assets" + toPath);
//        if (!string.IsNullOrEmpty(errorStr)) {
//            Debug.LogError(errorStr);
//        }

//        AssetDatabase.Refresh();
//    }


//    [MenuItem("AssetBundlew/Build", false, 51)]
//    public static void BuildAssets()
//    {
//        Build(EditorUserBuildSettings.activeBuildTarget);
//    }


//    private static void Build(BuildTarget param)
//    {
//        string fileFolder = Application.dataPath + "/" + AssetBundleUtility.fileFolderName;
//        string assetBundleFolder = fileFolder + "/" + AssetBundleUtility.GetPlatformName(param);

//        if (!Directory.Exists(assetBundleFolder))
//        {
//            Directory.CreateDirectory(assetBundleFolder);
//        }
//        //开始编译ab包
//        BuildPipeline.BuildAssetBundles(assetBundleFolder,options,param);
//        GenerateFileInfo(fileFolder);
//        Debug.Log("Finish build assetbundle");

//        AssetDatabase.Refresh();

//    }


//    private static void GenerateFileInfo(string param)
//    {

//    }

//}
