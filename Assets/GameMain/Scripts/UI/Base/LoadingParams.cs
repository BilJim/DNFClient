using GameFramework;

/// <summary>
/// 加载数据
/// </summary>
public class LoadingParams : IReference
{
    /// <summary>
    /// 获取加载场景进度。
    /// Unity 场景加载进度只会从 0-0.9 超过0.9即为加载完成
    /// </summary>
    public float Progress { get; set; }

    /// <summary>
    /// 提示文本
    /// </summary>
    public string Tips { get; set; }

    public bool LoadingComplete { get; set; }

    //建议实现一个静态的 Create 方法，将 Acquire 操作封装起来。便于外部调用
    public static LoadingParams Create(string tips)
    {
        LoadingParams poolInstance = ReferencePool.Acquire<LoadingParams>();
        poolInstance.Tips = tips;
        poolInstance.Progress = 0;
        poolInstance.LoadingComplete = false;
        return poolInstance;
    }

    public void Clear()
    {
        Tips = null;
    }
}