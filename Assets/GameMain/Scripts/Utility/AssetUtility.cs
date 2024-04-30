using GameFramework;

public static class AssetUtility
{
    
    //加载配置项路径
    public static string GetConfigAsset(string assetName, bool fromBytes)
    {
        return Utility.Text.Format("Assets/GameMain/Configs/{0}.{1}", assetName, fromBytes ? "bytes" : "txt");
    }
    
    //加载预制体路径
    public static string GetEntityAsset(string assetName)
    {
        return $"Assets/GameMain/Entities/{assetName}.prefab";
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
}