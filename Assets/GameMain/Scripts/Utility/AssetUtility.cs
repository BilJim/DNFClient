using System.IO;
using GameFramework;

/// <summary>
/// 资源路径
/// </summary>
public static class AssetUtility
{
    //加载配置项路径
    public static string GetConfigAsset(string assetName, bool fromBytes)
    {
        return Utility.Text.Format("Assets/GameMain/Configs/{0}.{1}", assetName, fromBytes ? "bytes" : "txt");
    }

    /// <summary>
    /// 数据表文本所在路径
    /// </summary>
    /// <param name="assetName">资源名</param>
    /// <param name="fromBytes">是否是二进制文件</param>
    /// <returns></returns>
    public static string GetDataTableAsset(string assetName, bool fromBytes)
    {
        return Utility.Text.Format("Assets/GameMain/DataTables/{0}.{1}", assetName, fromBytes ? "bytes" : "txt");
    }

    /// <summary>
    /// 字体资源所在路径
    /// </summary>
    public static string GetFontAsset(string assetName)
    {
        return Utility.Text.Format("Assets/GameMain/Fonts/{0}.ttf", assetName);
    }

    /// <summary>
    /// 本地化资源所在路径
    /// </summary>
    public static string GetLocalizationAsset(string assetName, bool fromBytes)
    {
        return Utility.Text.Format("Assets/GameMain/Localization/{0}/Dictionaries/{1}.{2}",
            GameEntry.Localization.Language, assetName, fromBytes ? "bytes" : "xml");
    }

    /// <summary>
    /// 预制体所在路径
    /// </summary>
    /// <param name="assetName">预制体名称</param>
    /// <param name="assetPath">预制体所在相对路径，注意，相对路径不能以分隔符开头</param>
    /// <returns></returns>
    public static string GetPrefabs(string assetName, string assetPath)
    {
        Utility.Path.GetRegularPath(Path.Combine("Assets/GameMain/Prefabs/", assetPath, assetName + ".prefab"));
        return Utility.Text.Format("{0}/{1}.prefab",assetPath, assetName);
    }
}