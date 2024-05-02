using GameFramework;

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
    
    public static string GetDictionaryAsset(string assetName, bool fromBytes)
    {
        return Utility.Text.Format("Assets/GameMain/Localization/{0}/Dictionaries/{1}.{2}", GameEntry.Localization.Language, assetName, fromBytes ? "bytes" : "xml");
    }
}